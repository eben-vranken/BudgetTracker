namespace BudgetTracker;

public class ExpenseCategory
{
    public string Name { get; set; }

    public ConsoleColor Color { get; set; }

    public ExpenseCategory(string name, string description, ConsoleColor color)
    {
        Name = name;
        Color = color;
    }
}