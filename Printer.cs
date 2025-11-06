namespace BudgetTracker;

public class Printer
{
    public void WriteLineColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public void WriteColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    public void WriteUnderlineColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.WriteLine(new string('-', text.Length));
        Console.ResetColor();
    }

    public void PrintBanner(string text, ConsoleColor color,  int padding = 2)
    {
        int width = text.Length + padding * 2;
        WriteLineColor("╔" + new string('═', width) +  "╗", color);
        WriteLineColor("║" + new string(' ', padding) + text + new string(' ', padding) + "║" , color);
        WriteLineColor("╚" + new string('═', width) +  "╝", color);

        Console.WriteLine();
    }

    public void PrintError(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}