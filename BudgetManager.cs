using System.Text.Json;

namespace BudgetTracker;

public class BudgetManager
{
    Printer _printer = new Printer();
    List<Expense> _expenses = new List<Expense>();
    List<ExpenseCategory> _categories = new List<ExpenseCategory>();
    
    private readonly string _expensesFilePath = "expenses.json";
    private readonly string _categoriesFilePath = "categories.json";
    
    public BudgetManager()
    {
        LoadData();
    }
    
    public void LoadData()
    {
        try
        {
            // Load expenses
            if (File.Exists(_expensesFilePath))
            {
                string json = File.ReadAllText(_expensesFilePath);
                _expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
            }

            // Load categories
            if (File.Exists(_categoriesFilePath))
            {
                string json = File.ReadAllText(_categoriesFilePath);
                _categories = JsonSerializer.Deserialize<List<ExpenseCategory>>(json) ?? new List<ExpenseCategory>();
            }
            
            else
            {
                _expenses = new List<Expense>();
                
                // Default categories
                _categories = new List<ExpenseCategory>
                {
                    new ExpenseCategory("Housing", "Rent, mortgage, utilities, and maintenance.", ConsoleColor.DarkRed),
                    new ExpenseCategory("Groceries", "Food, drinks, and household supplies.", ConsoleColor.Green),
                    new ExpenseCategory("Transportation", "Fuel, public transit, car payments, and maintenance.", ConsoleColor.Yellow),
                    new ExpenseCategory("Health", "Insurance, medicine, doctor visits, and fitness.", ConsoleColor.Cyan),
                    new ExpenseCategory("Entertainment", "Movies, games, streaming, and outings.", ConsoleColor.Magenta),
                    new ExpenseCategory("Dining Out", "Restaurants, cafés, takeout, and fast food.", ConsoleColor.DarkYellow),
                    new ExpenseCategory("Savings", "Emergency fund, investments, or future goals.", ConsoleColor.Blue),
                    new ExpenseCategory("Debt Payments", "Credit cards, loans, and interest.", ConsoleColor.Red),
                    new ExpenseCategory("Education", "Tuition, books, courses, or learning tools.", ConsoleColor.DarkGreen),
                    new ExpenseCategory("Personal Care", "Clothing, grooming, and hygiene products.", ConsoleColor.White),
                    new ExpenseCategory("Subscriptions", "Software, memberships, and online services.", ConsoleColor.Gray),
                    new ExpenseCategory("Gifts & Donations", "Presents, charity, or family support.", ConsoleColor.DarkCyan),
                    new ExpenseCategory("Miscellaneous", "Small or irregular expenses that don’t fit elsewhere.", ConsoleColor.DarkMagenta)
                };

                SaveData();
            }
        }
        catch (Exception e)
        {
            _printer.PrintError(e.Message);
        }
    }

    public void SaveData()
    {
        try
        {
            var options = new JsonSerializerOptions{ WriteIndented = true };
            
            // Save expenses
            string json = JsonSerializer.Serialize(_expenses, options);
            File.WriteAllText(_expensesFilePath, json);
            
            // Save categories
            json = JsonSerializer.Serialize(_categories, options);
            File.WriteAllText(_categoriesFilePath, json);
        }
        catch (Exception e)
        {
            _printer.PrintError(e.Message);
        }
    }

    public bool AddExpense(string name, string description, decimal amount, DateTime date, ExpenseCategory category)
    {
        
        try
        {
            int nextId = _expenses.Any() ? _expenses.Max(e => e.Id) + 1 : 1;
            Expense newExpense = new Expense(nextId, name, description, amount, date, category);
            
            _expenses.Add(newExpense);
            SaveData();
            return true;
        } catch(Exception e)
        {
            _printer.PrintError(e.Message);
            return false;
        }
    }

    public Expense[] GetExpenses()
    {
        return _expenses.ToArray();
    }

    public ExpenseCategory[] GetCategories()
    {
        return _categories.ToArray();
    }

    public bool DeleteExpense(int id)
    {
        try
        {
            _expenses.Remove(_expenses.FirstOrDefault(e => e.Id == id));
            SaveData();
            return true;
        }
        catch (Exception e)
        {
            _printer.PrintError(e.Message);
            return false;
        }
    }
}