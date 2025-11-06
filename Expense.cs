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
    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public decimal Amount
    {
        get => _amount;
        set => _amount = value;
    }

    public DateTime Date
    {
        get => _date;
        set => _date = value;
    }

    public ExpenseCategory Category
    {
        get => _category;
        set => _category = value;
    }
}