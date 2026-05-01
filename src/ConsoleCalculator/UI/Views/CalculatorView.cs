using ConsoleCalculator.Core.Engine;
using ConsoleCalculator.UI.Styling;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ConsoleCalculator.Views;

public class CalculatorView : View
{
    private readonly CalculatorEngine _calculatorEngine;

    private readonly View _calculatorContainer;
    private Label _expressionDisplay = null!;
    private Label _mainDisplay = null!;

    private const int Columns = 4;
    private const int ButtonWidth = 9;  // must be > 7
    private const int ButtonHeight = 3;
    private const int GapX = 1;
    private const int GapY = 1;
    private const int Rows = 5;

    private const int KeypadWidth = Columns * ButtonWidth + (Columns - 1) * GapX;
    private const int KeypadHeight = Rows * ButtonHeight + (Rows - 1) * GapY;

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
            Y = Pos.Center()
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

            SchemeName = ThemeNames.ExpressionDisplay
        };

        _mainDisplay = new Label // current input or result
        {
            X = 0,
            Y = Pos.Bottom(_expressionDisplay),
            Width = Dim.Fill(),
            Height = 1,
            TextAlignment = Alignment.End,
            Text = _calculatorEngine.State.CurrentInput,

            SchemeName = ThemeNames.MainDisplay
        };

        _calculatorContainer.Add(_expressionDisplay, _mainDisplay);
    }

    private void ConfigCalcButtons()
    {
        Button[] basicButtons =
        [
        new Button { Text = "(", SchemeName = ThemeNames.OperatorButton },
        new Button { Text = ")", SchemeName = ThemeNames.OperatorButton },
        new Button { Text = "DEL", SchemeName = ThemeNames.ActionButton },
        new Button { Text = "AC", SchemeName = ThemeNames.ActionButton },

        new Button { Text = "7", SchemeName = ThemeNames.NumberButton, },
        new Button { Text = "8", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "9", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "÷", SchemeName = ThemeNames.OperatorButton },

        new Button { Text = "4", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "5", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "6", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "×", SchemeName = ThemeNames.OperatorButton },

        new Button { Text = "1", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "2", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "3", SchemeName = ThemeNames.NumberButton },
        new Button { Text = "-", SchemeName = ThemeNames.OperatorButton },

        new Button { Text = "0", SchemeName = ThemeNames.NumberButton },
        new Button { Text = ".", SchemeName = ThemeNames.ActionButton },
        new Button { Text = "=", SchemeName = ThemeNames.EqualButton },
        new Button { Text = "+", SchemeName = ThemeNames.OperatorButton }
        ];

        var keypadContainer = new View
        {
            Width = KeypadWidth,
            Height = KeypadHeight,
            X = 0,
            Y = Pos.Bottom(_mainDisplay) + 1
        };

        for (int i = 0; i < basicButtons.Length; i++)
        {
            int row = i / Columns;
            int col = i % Columns;

            basicButtons[i].X = col * (ButtonWidth + GapX);
            basicButtons[i].Y = row * (ButtonHeight + GapY);
            basicButtons[i].Width = ButtonWidth;
            basicButtons[i].Height = ButtonHeight;

            basicButtons[i].TextAlignment = Alignment.Center;
            basicButtons[i].VerticalTextAlignment = Alignment.Center;
            basicButtons[i].NoDecorations = true;

            string buttonText = basicButtons[i].Text.ToString();

            if (buttonText == "DEL")
            {
                basicButtons[i].Accepted += (s, e) => OnDelClicked();
            }
            else if (buttonText == "AC")
            {
                basicButtons[i].Accepted += (s, e) => OnAcClicked();
            }
            else if (buttonText == "=")
            {
                basicButtons[i].Accepted += (s, e) => OnEqualClicked();
            }
            else
            {
                basicButtons[i].Accepted += (s, e) => OnButtonClicked(buttonText);
            }

            keypadContainer.Add(basicButtons[i]);
        }

        _calculatorContainer.Add(keypadContainer);
    }



    private void UpdateDisplay()
    {
        _expressionDisplay.Text = _calculatorEngine.State.CurrentExpression;

        _mainDisplay.Text = _calculatorEngine.State.CurrentInput;

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
}