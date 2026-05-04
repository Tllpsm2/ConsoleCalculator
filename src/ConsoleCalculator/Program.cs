using ConsoleCalculator.Core.Engine;
using ConsoleCalculator.Core.Engine.Operations;
using ConsoleCalculator.UI.Views;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;

ConfigurationManager.ThrowOnJsonErrors = true;
ConfigurationManager.Enable(ConfigLocations.All);
ConfigurationManager.Load(ConfigLocations.All);
ConfigurationManager.Apply();

var basicService = new BasicService();
var calculatorEngine = new CalculatorEngine("0", basicService);

using var app = Application.Create().Init();
using var mainView = new MainView(calculatorEngine);
app.Run(mainView);
