namespace BudgetTracker;

public class Expense
{
    private int _id;
    private string _name;
    private string _description;
    private decimal _amount;
    private DateTime _date;
    private ExpenseCategory _category;

    public Expense(int id, string name, string description, decimal amount, DateTime date, ExpenseCategory category)
    {
        _id = id;
        _name = name;
        _description = description;
        _amount = amount;
        _date = date;
        _category = category;
    }

    // Gets
    public int Id => _id;
    public string Name => _name;
    public string Description => _description;
    public decimal Amount => _amount;
    public DateTime Date => _date;
    public ExpenseCategory Category => _category;
}