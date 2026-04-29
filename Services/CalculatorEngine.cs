using ConsoleCalculator.Models;
using ConsoleCalculator.Services.Operations;

namespace ConsoleCalculator.Services;
public class CalculatorEngine
{
    public CalculatorState State { get; private set; }
    
    public CalculatorEngine(string initialExpression = "")
    {
        State = new CalculatorState(){ CurrentExpression = initialExpression };
    }
    public void ProcessInput(string input)
    {
        if (State.CurrentExpression == "Error")
            State.CurrentExpression = input;
        else
            State.CurrentExpression += input;
    }
    public void Evaluate()
    {
        if (string.IsNullOrWhiteSpace(State.CurrentExpression))
            return;
        try
        {
            double result = BasicService.EvaluateComplex(State.CurrentExpression);

            if (double.IsInfinity(result) || double.IsNaN(result))
                throw new DivideByZeroException();

            State.CurrentExpression = result.ToString();
        }
        catch (Exception)
        {
            State.CurrentExpression = "Error";
        }
    }

    public void ClearAll()
    {
        State.CurrentExpression = "";
    }
}