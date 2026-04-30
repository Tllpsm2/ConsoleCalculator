using Terminal.Gui.Configuration;
using ConsoleCalculator.Views;
using Terminal.Gui.App;

ConfigurationManager.ThrowOnJsonErrors = true;
ConfigurationManager.Enable(ConfigLocations.All);
ConfigurationManager.Load(ConfigLocations.All);
ConfigurationManager.Apply();


using var app = Application.Create().Init();
using var mainView = new ConfigMainView();
app.Run(mainView);
