using System.Globalization;
using System.Text;

namespace BudgetTracker;

class Program
{
    /// Track income/expenses with categories, monthly reports, and data persistence.
    
    static readonly Printer Printer = new Printer();
    static readonly BudgetManager BudgetManager = new BudgetManager();
    
    private const string Version = "0.0.1";
    
    // Display Properties
    private const ConsoleColor HeaderColor = ConsoleColor.Yellow;
    private const ConsoleColor ListKeyColor = ConsoleColor.Magenta;
    private const ConsoleColor ListItemColor = ConsoleColor.Red;
    private const ConsoleColor PromptColor = ConsoleColor.Green;
    private const ConsoleColor StatusColor = ConsoleColor.Cyan;
    
    // Numerical
    const int ListPadding = 3;
    
    // Display
    private static string _statusMessage = "";
    
    private static readonly Dictionary<string, string> MainMenuOptions = new()
    {
        { "M", "Manage Expense" },
        { "R", "View Reports" },
        { "E", "Exit" }
    };

    private static readonly Dictionary<string, string> ExpenseOptions = new()
    {
        { "N", "New Expense"},
        { "V", "View Expenses" },
        { "E", "Edit Expense"},
        { "D", "Delete Expense" },
        { "B", "Back"}
    };

    private static readonly Dictionary<string, string> ReportOptions = new()
    {
        { "A", "Average" },
        { "B", "Back" }
    };
    
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        PrintMenu($"Budget Tracker - Version: {Version}", "Options:", MainMenuOptions);
        
        string userInput = GetValidUserInput("Enter action", MainMenuOptions.Keys.ToArray());

        // Main Menu Loop
        while (userInput != "E")
        {
            switch (userInput)
            {
                // Manage expenses
                case "M":
                    string expenseInput = "";
                    while(expenseInput != "B")
                    {
                        PrintMenu("Expenses", "Options:", ExpenseOptions);
                        expenseInput = GetValidUserInput("Enter action", ExpenseOptions.Keys.ToArray());

                        switch (expenseInput)
                        {
                            case "N":
                                NewExpense();
                                break;
                            case "V":
                                ViewExpenses();
                                break;
                            case "D":
                                DeleteExpense();
                                break;
                        }
                    }
                    break;
                // View reports
                case "R":
                    PrintMenu("Reports","Options:", ReportOptions);
                    string reportInput = GetValidUserInput("Enter action", ReportOptions.Keys.ToArray());
                    while(reportInput != "B")
                    {
                        reportInput = GetValidUserInput("Enter action", ReportOptions.Keys.ToArray());
                    }
                    break;
            }
            
            Console.Clear();
            
            PrintMenu($"Budget Tracker - Version: {Version}", "Options", MainMenuOptions);
            
            userInput = GetValidUserInput("Enter action", MainMenuOptions.Keys.ToArray());
        }

        Printer.WriteLineColor("Exiting", ConsoleColor.Red);
    }

    static void PrintMenu(string banner, string menuTitle, Dictionary<string, string> options)
    {
        Console.Clear();
        
        Printer.PrintBanner(banner, HeaderColor);
        
        Printer.WriteLineColor(menuTitle, HeaderColor);
        
        foreach (var option in options)
        {
            Printer.WriteColor(new string(' ', ListPadding), ListKeyColor);
            Printer.WriteColor($"[{option.Key}] ", ListKeyColor);
            Printer.WriteLineColor(option.Value, ListItemColor);
        }
        
        if (!string.IsNullOrEmpty(_statusMessage))
        {
            Printer.WriteLineColor($"\n{_statusMessage}", StatusColor);
        }

        _statusMessage = "";
        
        Console.WriteLine();
    }

    static string GetValidUserInput(string prompt, string[] validInputs)
    {
        while (true)
        {
            Printer.WriteColor($"{prompt}: ", PromptColor);
            string userInput = Console.ReadLine() ?? "";
            
            if(validInputs.Contains(userInput.ToUpper())) return userInput.ToUpper();
            
            Printer.WriteLineColor($"Invalid input: ", ConsoleColor.Red);
        }
    }

    static string GetUserInput(string prompt, bool required=true, string optionalHint = "Leave empty for default")
    {
        while (true)
        {
            Printer.WriteColor($"{prompt}", PromptColor);

            if (!required)
            {
                Printer.WriteColor($" ({optionalHint})", PromptColor);
            }
            
            Printer.WriteColor(": ",  PromptColor);
            
            string userInput = Console.ReadLine() ?? "";

            if (required)
            {
                if (!string.IsNullOrEmpty(userInput))
                {
                    return userInput;
                }
                
                Printer.PrintError("Input cannot be empty!");
            }
            else
            {
                return userInput;
            }
        }
    }

    static decimal GetNumericInput(string prompt, bool required=true, string optionalHint = "Leave empty for default")
    {
        while (true)
        {
            Printer.WriteColor($"{prompt}", PromptColor);

            if (!required)
            {
                Printer.WriteColor($" ({optionalHint})", PromptColor);
            }
            
            Printer.WriteColor(": ",  PromptColor);

            try
            {

                string userInput = Console.ReadLine() ?? "";

                if (required)
                {
                    if (!string.IsNullOrEmpty(userInput))
                    {
                        return decimal.Parse(userInput);
                    }

                    Printer.PrintError("Input cannot be empty!");
                }
                else
                {
                    return decimal.Parse(userInput);
                }
            }
            catch (Exception e)
            {
                Printer.PrintError(e.Message);
            }
        }
    }
    
    static void NewExpense()
    {
        Console.Clear();
        Printer.PrintBanner("New Expense", HeaderColor);
        string expenseName = GetUserInput("Enter expense name");
        string expenseDescription = GetUserInput("Enter expense description", false);
        decimal amount = GetNumericInput("Enter amount");
        string date = GetUserInput("Enter expense date", false, "Leave blank for today");

        // Set date to today if empty
        if (string.IsNullOrEmpty(date))
        {
            date = DateTime.Now.ToString("dd/MM/yyyy");
        }
        
        ExpenseCategory[] categories = BudgetManager.GetCategories();
        string[] validCategories = new string[categories.Length];
        
        Printer.WriteLineColor("Categories:", PromptColor);
        
        // Print categories
        for (int i = 0; i < categories.Length; i++)
        {
            Printer.WriteColor($"{i + 1}. ", ListKeyColor);
            Printer.WriteLineColor(categories[i].Name, categories[i].Color);
            
            validCategories[i] = (i + 1).ToString();
        }

        Console.WriteLine();

        int selectedCategory = int.Parse(GetValidUserInput("Enter category", validCategories));
        
        bool success = BudgetManager.AddExpense(expenseName, expenseDescription, amount, DateTime.Parse(date), categories[selectedCategory - 1]);

        _statusMessage = success ? "Expense added successfully!" : "Expense could not be added!";
    }

    static void ViewExpenses()
    {
        Console.Clear();
        Printer.PrintBanner("All Expenses", HeaderColor);
        Expense[] expenses = BudgetManager.GetExpenses();

        if (expenses.Length == 0)
        {
            Printer.WriteLineColor("No expenses found!\n", HeaderColor);
        }
        else
        {
            for (int i = 0; i < expenses.Length; i++)
            {
                Printer.WriteColor($"{expenses[i].Id}. ", ListKeyColor);
                Printer.WriteColor(expenses[i].Name, ListItemColor);
                
                Printer.WriteColor(" - ", ListKeyColor);
                Printer.WriteLineColor($"\u20AC{expenses[i].Amount.ToString(CultureInfo.CurrentCulture)}", ListItemColor);
                
                // Calculated underline
                Printer.WriteLineColor(new string('-', ("1. " + expenses[i].Name + " - " + $"\u20AC{expenses[i].Amount.ToString(CultureInfo.CurrentCulture)}").Length), ListKeyColor);

                Printer.WriteColor("Description: ", ListKeyColor);
                string description = !string.IsNullOrEmpty(expenses[i].Description)
                    ? expenses[i].Description
                    : "No description!";
                Printer.WriteLineColor($"{description}", ListItemColor);
                
                
                Printer.WriteColor($"Category:    ", ListKeyColor);
                Printer.WriteLineColor(expenses[i].Category.Name, expenses[i].Category.Color);
                
                Printer.WriteColor($"Date:        ", ListKeyColor);
                Printer.WriteLineColor(expenses[i].Date.ToShortDateString() , ListItemColor);

                Console.WriteLine();
            }
        }
        
        Printer.WriteLineColor("Enter any key to return", PromptColor);
        Console.ReadKey();
    }
    
    static void DeleteExpense()
    {
        Console.Clear();
        Printer.PrintBanner("Delete Expense", HeaderColor);
        
        Expense[] expenses = BudgetManager.GetExpenses();
        int size = expenses.Length;
        string[] validInputs = new string[size + 1];
        
        for (int i = 0; i < size; i++)
        {
            validInputs[i] = expenses[i].Id.ToString();
        }
        
        validInputs[validInputs.Length - 1] = "EXIT";
        
        string userInput = GetValidUserInput("Enter expense id (Exit to return)",  validInputs);

        if (userInput != "EXIT")
        {
            bool success = BudgetManager.DeleteExpense(int.Parse(userInput));
            
            _statusMessage = success ? "Expense deleted successfully!" : "Expense could not be deleted!";
        }
    }
}