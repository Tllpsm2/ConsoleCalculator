using NCalc;

namespace ConsoleCalculator.Core.Engine.Operations;

public static class BasicService
{
    public static double EvaluateComplex(string expression)
    {
        try
        {
            return Convert.ToDouble(new
            Expression(Normalize(expression)).Evaluate());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Invalid expression", ex);
        }
    }

    private static string Normalize(string expression) => expression
        .Replace("÷", "/")
        .Replace("×", "*")
        .Replace(",", ".");
}

