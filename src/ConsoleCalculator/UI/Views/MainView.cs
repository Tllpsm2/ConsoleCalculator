using ConsoleCalculator.Core.Engine;
using ConsoleCalculator.UI.Styling;
using ConsoleCalculator.Views;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace ConsoleCalculator.UI.Views;

public class ConfigMainView : Window
{
    private readonly Tabs _tabContainer;
    private readonly Tabs _calculatorTab;
    private readonly Tabs _historyTab;
    private readonly Tabs _settingsTab;

    public ConfigMainView()
    {
        Title = "Console Calculator";
        SchemeName = ThemeNames.AppBackground;

        _calculatorTab = new Tabs { Title = "Calculator", SchemeName = ThemeNames.CalculatorFrame };
        _historyTab = new Tabs { Title = "History", SchemeName = ThemeNames.CalculatorFrame };
        _settingsTab = new Tabs { Title = "Settings", SchemeName = ThemeNames.CalculatorFrame };

        _tabContainer = new Tabs
        {
            SchemeName = ThemeNames.NavigationTabs,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1 // Leave space for the status bar
        };

        _tabContainer.Add(_calculatorTab, _historyTab, _settingsTab);
        _calculatorTab.Add(new CalculatorView(new CalculatorEngine()));

        Add(_tabContainer);

        SetupStatusBar();
    }

    private void SetupStatusBar()
    {
        Shortcut[] shortcuts =
        [
            new Shortcut { Title = "_Quit", Key = Key.Esc, Action = () => App?.RequestStop() },
            new Shortcut { Title = "_Next Tab", Key = Key.Tab, Action = () => App?.Navigation?.AdvanceFocus(NavigationDirection.Forward, null) }
        ];

        var statusBar = new StatusBar(shortcuts)
        {
            SchemeName = ThemeNames.StatusBar
        };

        Add(statusBar);
    }

}