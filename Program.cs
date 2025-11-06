using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BudgetTracker;

class Program
{
    /// Track income/expenses with categories, monthly reports, and data persistence.
    
    static Printer _printer = new Printer();
    static BudgetManager _budgetManager = new BudgetManager();
    
    private const string Version = "0.0.1";
    
    // Display Properties
    const ConsoleColor HeaderColor = ConsoleColor.Yellow;
    const ConsoleColor ListKeyColor = ConsoleColor.Magenta;
    const ConsoleColor ListItemColor = ConsoleColor.Red;
    const ConsoleColor PromptColor = ConsoleColor.Green;
    const ConsoleColor StatusColor = ConsoleColor.Cyan;
    
    // Numerical
    const int ListPadding = 3;
    
    // Display
    static string StatusMessage = "";
    
    static private Dictionary<string, string> MainMenuOptions = new()
    {
        { "M", "Manage Expense" },
        { "R", "View Reports" },
        { "E", "Exit" }
    };

    static private Dictionary<string, string> ExpenseOptions = new()
    {
        { "N", "New Expense"},
        { "E", "Edit Expense"},
        { "D", "Delete Expense" },
        { "V", "View Expenses" },
        { "B", "Back"}
    };

    static private Dictionary<string, string> ReportOptions = new()
    {
        { "A", "Average" },
        { "B", "Back" }
    };
    
    static void Main(string[] args)
    {
        PrintMenu($"Budget Tracker - Version: {Version}", "Options:", MainMenuOptions);
        
        string userInput = GetValidUserInput("Enter action", MainMenuOptions.Keys.ToArray());

        // Main Menu Loop
        while (userInput != "E")
        {
            switch (userInput)
            {
                // Manage expenses
                case "M":
                    PrintMenu("Expenses", "Options:", ExpenseOptions);
                    string expenseInput = GetValidUserInput("Enter action", ExpenseOptions.Keys.ToArray());
                    while(expenseInput != "B")
                    {
                        expenseInput = GetValidUserInput("Enter action", ExpenseOptions.Keys.ToArray());

                        switch (expenseInput)
                        {
                            case "N":
                                NewExpense();
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

            if (!string.IsNullOrEmpty(StatusMessage))
            {
                _printer.WriteLineColor(StatusMessage, StatusColor);
            }
            
            userInput = GetValidUserInput("Enter action", MainMenuOptions.Keys.ToArray());
        }

        _printer.WriteLineColor("Exiting", ConsoleColor.Red);
    }

    static void PrintMenu(string banner, string menuTitle, Dictionary<string, string> options)
    {
        Console.Clear();
        
        _printer.PrintBanner(banner, HeaderColor);
        
        _printer.WriteLineColor(menuTitle, HeaderColor);
        
        foreach (var option in options)
        {
            _printer.WriteColor(new string(' ', ListPadding), ListKeyColor);
            _printer.WriteColor($"[{option.Key}] ", ListKeyColor);
            _printer.WriteLineColor(option.Value, ListItemColor);
        }
        
        Console.WriteLine();
    }

    static string GetValidUserInput(string prompt, string[] validInputs)
    {
        while (true)
        {
            _printer.WriteColor($"{prompt}: ", PromptColor);
            string userInput = Console.ReadLine() ?? "";
            
            if(validInputs.Contains(userInput.ToUpper())) return userInput.ToUpper();
            
            _printer.WriteLineColor($"Invalid input: ", ConsoleColor.Red);
        }
    }

    static string GetUserInput(string prompt, bool required=true)
    {
        while (true)
        {
            _printer.WriteColor($"{prompt}", PromptColor);

            if (!required)
            {
                _printer.WriteColor($" (Leave empty for default)", PromptColor);
            }
            
            _printer.WriteColor(": ",  PromptColor);
            
            string userInput = Console.ReadLine() ?? "";

            if (required)
            {
                if (!string.IsNullOrEmpty(userInput))
                {
                    return userInput;
                }
                
                _printer.PrintError("Input cannot be empty!");
            }
            else
            {
                return userInput;
            }
        }
    }

    static decimal GetNumericInput(string prompt, bool required=true)
    {
        while (true)
        {
            _printer.WriteColor($"{prompt}", PromptColor);

            if (!required)
            {
                _printer.WriteColor($"(Leave empty for default", PromptColor);
            }
            
            _printer.WriteColor(": ",  PromptColor);

            try
            {

                decimal userInput = decimal.Parse(Console.ReadLine() ?? "");

                if (required)
                {
                    if (!string.IsNullOrEmpty(userInput.ToString()))
                    {
                        return userInput;
                    }

                    _printer.PrintError("Input cannot be empty!");
                }
                else
                {
                    return userInput;
                }
            }
            catch (Exception e)
            {
                _printer.PrintError(e.Message);
            }
        }
    }
    
    static void NewExpense()
    {
        string expenseName = GetUserInput("Enter name");
        string expenseDescription = GetUserInput("Enter description", false);
        decimal amount = GetNumericInput("Enter amount");
        string date = GetUserInput("Enter date");
        ExpenseCategory category = new ExpenseCategory(GetUserInput("Enter category"), "");
        
        _budgetManager.AddExpense(expenseName, expenseDescription, amount, DateTime.Parse(date), category);
    }
}