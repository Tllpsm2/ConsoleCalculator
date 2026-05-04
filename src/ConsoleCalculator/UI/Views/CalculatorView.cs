using ConsoleCalculator.Core.Engine;
using ConsoleCalculator.UI.Styling;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ConsoleCalculator.UI.Views;

public class CalculatorView : View
{
    private const int Columns = 4;
    private const int ButtonWidth = 9; // must be > 7
    private const int ButtonHeight = 3;
    private const int GapX = 1;
    private const int GapY = 1;
    private const int Rows = 5;

    private const int KeypadWidth = Columns * ButtonWidth + (Columns - 1) * GapX;
    private const int KeypadHeight = Rows * ButtonHeight + (Rows - 1) * GapY;

    private const string DeleteButtonText = "DEL";
    private const string ClearButtonText = "AC";
    private const string EqualButtonText = "=";

    private readonly View _calculatorContainer;
    private readonly CalculatorEngine _calculatorEngine;
    private Label _expressionDisplay = null!;
    private Label _mainDisplay = null!;

    public CalculatorView(CalculatorEngine calculatorEngine)
    {
        _calculatorEngine = calculatorEngine;
        SchemeName = ThemeNames.CalculatorFrame;

        Width = Dim.Fill();
        Height = Dim.Fill();

        // Fixed-width container
        _calculatorContainer = new View
        {
            Width = KeypadWidth,
            Height = Dim.Auto(),
            X = Pos.Center(),
            Y = Pos.Center(),
        };

        Add(_calculatorContainer);

        ConfigCalcDisplay();
        ConfigCalcButtons();
    }

    private void ConfigCalcDisplay()
    {
        _expressionDisplay = new Label // past expression
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = 2,

            VerticalTextAlignment = Alignment.End,
            TextAlignment = Alignment.End,
            Text = _calculatorEngine.State.CurrentExpression,

            SchemeName = ThemeNames.ExpressionDisplay,
        };

        _mainDisplay = new Label // current input or result
        {
            X = 0,
            Y = Pos.Bottom(_expressionDisplay),
            Width = Dim.Fill(),
            Height = 1,
            TextAlignment = Alignment.End,
            Text = _calculatorEngine.State.CurrentInput,

            SchemeName = ThemeNames.MainDisplay,
        };

        _calculatorContainer.Add(_expressionDisplay, _mainDisplay);
    }

    private void ConfigCalcButtons()
    {
        var keypadContainer = CreateKeypadContainer();

        (string Text, string SchemeName)[] buttonSpecs =
        [
            ("(", ThemeNames.OperatorButton),
            (")", ThemeNames.OperatorButton),
            (DeleteButtonText, ThemeNames.ActionButton),
            (ClearButtonText, ThemeNames.ActionButton),

            ("7", ThemeNames.NumberButton),
            ("8", ThemeNames.NumberButton),
            ("9", ThemeNames.NumberButton),
            ("÷", ThemeNames.OperatorButton),

            ("4", ThemeNames.NumberButton),
            ("5", ThemeNames.NumberButton),
            ("6", ThemeNames.NumberButton),
            ("×", ThemeNames.OperatorButton),

            ("1", ThemeNames.NumberButton),
            ("2", ThemeNames.NumberButton),
            ("3", ThemeNames.NumberButton),
            ("-", ThemeNames.OperatorButton),

            ("0", ThemeNames.NumberButton),
            (".", ThemeNames.ActionButton),
            (EqualButtonText, ThemeNames.EqualButton),
            ("+", ThemeNames.OperatorButton),
        ];

        for (var index = 0; index < buttonSpecs.Length; index++)
        {
            var button = CreateCalculatorButton(buttonSpecs[index], index);
            keypadContainer.Add(button);
        }

        _calculatorContainer.Add(keypadContainer);
    }

    private View CreateKeypadContainer()
    {
        return new View
        {
            Width = KeypadWidth,
            Height = KeypadHeight,
            X = 0,
            Y = Pos.Bottom(_mainDisplay) + 1,
        };
    }

    private Button CreateCalculatorButton((string Text, string SchemeName) spec, int index)
    {
        var row = index / Columns;
        var column = index % Columns;

        var button = new Button
        {
            Text = spec.Text,
            SchemeName = spec.SchemeName,
            X = column * (ButtonWidth + GapX),
            Y = row * (ButtonHeight + GapY),
            Width = ButtonWidth,
            Height = ButtonHeight,
            TextAlignment = Alignment.Center,
            VerticalTextAlignment = Alignment.Center,
            NoDecorations = true,
        };

        button.Accepted += spec.Text switch
        {
            DeleteButtonText => (_, _) => OnDelClicked(),
            ClearButtonText => (_, _) => OnAcClicked(),
            EqualButtonText => (_, _) => OnEqualClicked(),
            _ => (_, _) => OnButtonClicked(spec.Text),
        };

        return button;
    }

    private void UpdateDisplay()
    {
        _expressionDisplay.Text = _calculatorEngine.State.CurrentExpression;

        _mainDisplay.Text = string.IsNullOrWhiteSpace(_calculatorEngine.State.CurrentInput)
            ? "0"
            : _calculatorEngine.State.CurrentInput;

        _expressionDisplay.SetNeedsDraw();
        _mainDisplay.SetNeedsDraw();
    }

    private void OnButtonClicked(string buttonText)
    {
        _calculatorEngine.ProcessInput(buttonText);
        UpdateDisplay();
    }

    private void OnDelClicked()
    {
        _calculatorEngine.DeleteLast();
        UpdateDisplay();
    }

    private void OnAcClicked()
    {
        _calculatorEngine.AllClear();
        UpdateDisplay();
    }

    private void OnEqualClicked()
    {
        _calculatorEngine.Evaluate();
        UpdateDisplay();
    }

    protected override bool OnKeyDown(Key key)
    {
        var p = (char)key.KeyCode;

        if (char.IsDigit(p) || "+-.,()".Contains(p, StringComparison.Ordinal))
        {
            OnButtonClicked(p.ToString());
            return true;
        }

        if ("/:÷".Contains(p, StringComparison.Ordinal))
        {
            OnButtonClicked("÷");
            return true;
        }

        if ("*xX".Contains(p, StringComparison.Ordinal))
        {
            OnButtonClicked("×");
            return true;
        }

        if (key == Key.Enter)
        {
            OnEqualClicked();
            return true;
        }

        if (key == Key.Backspace)
        {
            OnDelClicked();
            return true;
        }

        if (key == Key.C)
        {
            OnAcClicked();
            return true;
        }

        return base.OnKeyDown(key);
    }
}