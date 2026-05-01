using ConsoleCalculator.Core.Models;
using ConsoleCalculator.Core.Engine.Operations;

namespace ConsoleCalculator.Core.Engine;

public class CalculatorEngine
{
    public CalculatorState State { get; private set; }

    public CalculatorEngine(string initialExpression = " ")
    {
        State = new CalculatorState() { CurrentInput = initialExpression, CurrentExpression = " " };
    }

    public void Evaluate()
    {
        if (string.IsNullOrWhiteSpace(State.CurrentInput))
            return;

        var expression = State.CurrentInput;

        try
        {
            double result = BasicService.EvaluateComplex(expression);

            if (double.IsInfinity(result) || double.IsNaN(result))
                throw new InvalidOperationException("Result is undefined");

            State.CurrentInput = result.ToString("0.########"); // limit to 8 decimals
            State.CurrentExpression = expression;
        }
        catch (Exception)
        {
            State.CurrentInput = "Error";
            State.CurrentExpression = " ";
        }
    }
    public void ProcessInput(string input)
    {
        if (State.CurrentInput == "Error")
            State.CurrentInput = input;
        else
            State.CurrentInput += input;
    }
    public void AllClear()
    {
        State.CurrentExpression = " ";
        State.CurrentInput = " ";
    }
    public void DeleteLast()
    {
        if (string.IsNullOrEmpty(State.CurrentInput)) return;

        State.CurrentInput = State.CurrentInput.Length > 1
            ? State.CurrentInput[..^1]
            : " ";
    }
}