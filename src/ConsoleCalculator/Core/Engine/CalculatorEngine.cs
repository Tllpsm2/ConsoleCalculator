using System.Globalization;
using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;

namespace ConsoleCalculator.Core.Engine;

public class CalculatorEngine(string initialExpression, IBasicService basicService)
{
    private const int MaxLength = 256;
    private const string ErrorMessage = "Error";
    private bool _hasEvaluated;

    public CalculatorState State { get; } = new()
    {
        CurrentInput = initialExpression,
        CurrentExpression = string.Empty
    };

    public void Evaluate()
    {
        if (string.IsNullOrWhiteSpace(State.CurrentInput) || State.CurrentInput is ErrorMessage)
            return;

        var expression = State.CurrentInput;

        try
        {
            var result = basicService.EvaluateComplex(expression);

            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                SetErrorState();
                return;
            }

            State.CurrentInput = result.ToString("0.########", CultureInfo.InvariantCulture); // limit to 8 decimals
            State.CurrentExpression = expression;

            _hasEvaluated = true;
        }
        catch (InvalidOperationException)
        {
            SetErrorState();
        }
    }

    public void ProcessInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return;

        if (State.CurrentInput is ErrorMessage)
        {
            AllClear();
            State.CurrentInput = input;
            return;
        }

        if (State.CurrentInput.Length + input.Length > MaxLength)
            return;

        var isDigit = char.IsDigit(input[0]);
        if (_hasEvaluated)
        {
            State.CurrentInput = isDigit
                ? input
                : State.CurrentInput + input;
            _hasEvaluated = false;
        }
        else if (State.CurrentInput is "0" && isDigit)
        {
            State.CurrentInput = input;
        }
        else
        {
            State.CurrentInput += input;
        }
    }

    public void AllClear()
    {
        State.CurrentExpression = string.Empty;
        State.CurrentInput = string.Empty;
        _hasEvaluated = false;
    }

    public void DeleteLast()
    {
        if (State.CurrentInput is ErrorMessage)
        {
            AllClear();
            return;
        }

        if (string.IsNullOrEmpty(State.CurrentInput))
            return;

        State.CurrentInput =
            State.CurrentInput.Length > 1
                ? State.CurrentInput[..^1]
                : string.Empty;

        _hasEvaluated = false;
    }

    public void SetErrorState()
    {
        State.CurrentInput = ErrorMessage;
        State.CurrentExpression = string.Empty;
        _hasEvaluated = false;
    }
}