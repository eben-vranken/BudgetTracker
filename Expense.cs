namespace BudgetTracker;

public class Expense
{
    private string _name;
    private string _description;
    private decimal _amount;
    private DateTime _date;
    private ExpenseCategory _category;

    public Expense(string name, string description, decimal amount, DateTime date, ExpenseCategory category)
    {
        _name = name;
        _description = description;
        _amount = amount;
        _date = date;
        _category = category;
    }
}