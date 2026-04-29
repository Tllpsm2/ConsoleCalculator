namespace ConsoleCalculator.Models;

public class HistoryEntry
{
    public DateTime Timestamp { get; set; }
    public string Equation { get; set; } = string.Empty;
    public double Result { get; set; }
}