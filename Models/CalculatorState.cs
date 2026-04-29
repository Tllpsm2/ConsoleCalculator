namespace ConsoleCalculator.Models;

public class CalculatorState
{
    public double OperandA { get; set; }
    public double OperandB { get; set; }
    public string CurrentInput { get; set; } = string.Empty;
    public string PendingOp { get; set; } = string.Empty;
    public string CurrentExpression { get; set; } = string.Empty;
    public bool IsNewEntry { get; set; }
}