using ConsoleCalculator.Core.Interfaces;
using NCalc;

namespace ConsoleCalculator.Core.Engine.Operations;

public class BasicService : IBasicService
{
    public double EvaluateComplex(string expression)
    {
        try
        {
            var raw = new Expression(Normalize(expression)).Evaluate();

            return raw switch
            {
                int i => i,
                double d => d,
                _ => throw new InvalidOperationException("Unexpected result type")
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Invalid expression", ex);
        }
    }

    private static string Normalize(string expression)
    {
        return expression
            .Replace('÷', '/')
            .Replace('×', '*')
            .Replace(',', '.');
    }
}