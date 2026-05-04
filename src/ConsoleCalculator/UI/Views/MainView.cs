using ConsoleCalculator.Core.Engine;
using ConsoleCalculator.UI.Styling;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ConsoleCalculator.UI.Views;

public class MainView : Window
{
    public MainView(CalculatorEngine calculatorEngine)
    {
        Title = "Console Calculator";
        SchemeName = ThemeNames.AppBackground;

        var calculatorTab = new Tabs { Title = "Calculator", SchemeName = ThemeNames.CalculatorFrame };
        var historyTab = new Tabs { Title = "History", SchemeName = ThemeNames.CalculatorFrame };
        var settingsTab = new Tabs { Title = "Settings", SchemeName = ThemeNames.CalculatorFrame };

        var tabContainer = new Tabs
        {
            SchemeName = ThemeNames.NavigationTabs,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1, // Leave space for the status bar
        };

        tabContainer.Add(calculatorTab, historyTab, settingsTab);
        calculatorTab.Add(new CalculatorView(calculatorEngine));

        Add(tabContainer);

        SetupStatusBar();
    }

    private void SetupStatusBar()
    {
        Shortcut[] shortcuts =
        [
            new()
            {
                Title = "_Quit",
                Key = Key.Esc,
                Action = () => App?.RequestStop(),
            },
            new()
            {
                Title = "_Next Tab",
                Key = Key.Tab,
                Action = () => App?.Navigation?.AdvanceFocus(NavigationDirection.Forward, behavior: null),
            },
        ];

        var statusBar = new StatusBar(shortcuts) { SchemeName = ThemeNames.StatusBar };

        Add(statusBar);
    }
}