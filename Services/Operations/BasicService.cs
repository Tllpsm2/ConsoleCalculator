namespace ConsoleCalculator.Services.Operations;

using System.Data;
using System.Linq.Expressions;

public static class BasicService
{
    public static double Add(double a, double b) => a + b;
    public static double Subtract(double a, double b) => a - b;
    public static double Multiply(double a, double b) => a * b;
    public static double Divide(double a, double b) => a / b;

    public static double EvaluateComplex(string expression)
    {
        try
        {
            string cleanExpression = expression
                .Replace("÷", "/")
                .Replace("×", "*")
                .Replace(",", ".");

            var ncalcExpression = new NCalc.Expression(cleanExpression);
            var result = ncalcExpression.Evaluate();
            return Convert.ToDouble(result);
        }
        catch (Exception)
        {
            throw new InvalidOperationException("Invalid expression");
        }
    }
}