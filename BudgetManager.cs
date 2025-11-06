using System.Text.Json;

namespace BudgetTracker;

public class BudgetManager
{
    Printer _printer = new Printer();
    List<Expense> _expenses = new List<Expense>();
    private readonly string _filePath = "expenses.json";
    
    public BudgetManager()
    {
        LoadExpenses();
    }
    
    public void LoadExpenses()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
            }
            else
            {
                _expenses = new List<Expense>();
                SaveExpenses();
            }
        }
        catch (Exception e)
        {
            _printer.PrintError(e.Message);
        }
    }

    public void SaveExpenses()
    {
        try
        {
            var options = new JsonSerializerOptions{ WriteIndented = true };
            string json = JsonSerializer.Serialize(_expenses, options);
            File.WriteAllText(_filePath, json);
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
            Expense newExpense = new Expense(name, description, amount, date, category);
            
            _expenses.Add(newExpense);
            SaveExpenses();
            return true;
        } catch(Exception e)
        {
            _printer.PrintError(e.Message);
            return false;
        }
    }
}