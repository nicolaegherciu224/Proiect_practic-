using System.Windows;
using EngineeringManagementApp.Services;
using EngineeringManagementApp.Views;

namespace EngineeringManagementApp;

public partial class App : Application
{
  private async void Application_Startup(object sender, StartupEventArgs e)
  {
    if (PasswordResetCli.IncearcaExecutare(e.Args))
    {
      Shutdown();
      return;
    }

    AppServices.Initialize();

    MessageBox.Show($"DB path: {AppServices.DatabaseConnection.CaleFisierBazaDate}", "DB Path", MessageBoxButton.OK, MessageBoxImage.Information);

    try
    {
      await AppServices.DatabaseInitializer.InitializeAsync().ConfigureAwait(true);
    }
    catch (Exception ex)
    {
      MessageBox.Show(
        $"Nu s-a putut inițializa baza de date SQLite.\n\nCale: {AppServices.DatabaseConnection.CaleFisierBazaDate}\n\nDetalii: {ex.Message}",
        "Eroare bază de date",
        MessageBoxButton.OK,
        MessageBoxImage.Error);
      Shutdown();
      return;
    }

    var loginWindow = new LoginWindow();
    loginWindow.Show();
  }

  private void Application_Exit(object sender, ExitEventArgs e)
  {
    UserSession.End();
    AppServices.Dispose();
  }
}
