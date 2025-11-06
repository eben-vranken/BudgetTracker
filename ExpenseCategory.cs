namespace BudgetTracker;

public class ExpenseCategory
{
    public string Name { get; set; }
    public string Description { get; set; }

    public ConsoleColor Color { get; set; }
    
    public ExpenseCategory() { }

    public ExpenseCategory(string name, string description, ConsoleColor color)
    {
        Name = name;
        Description = description;
        Color = color;
    }
}