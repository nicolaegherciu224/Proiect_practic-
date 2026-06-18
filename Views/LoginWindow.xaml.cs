using System.Windows;
using System.Windows.Input;
using EngineeringManagementApp.Services;

namespace EngineeringManagementApp.Views;

/// <summary>
/// Fereastra de autentificare cu mascare parolă și verificare rol.
/// </summary>
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => LoginTextBox.Focus();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await IncearcaAutentificareAsync();
    }

    private async void Input_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            await IncearcaAutentificareAsync();
        }
    }

    private async Task IncearcaAutentificareAsync()
    {
        ClearError();
        SetBusy(true);

        try
        {
            var login = LoginTextBox.Text;
            var parola = PasswordBox.Password;

            var rezultat = await AppServices.Auth.LoginAsync(login, parola).ConfigureAwait(true);

            if (!rezultat.Success || rezultat.Account is null)
            {
                AfiseazaEroare(rezultat.Message);
                PasswordBox.Clear();
                PasswordBox.Focus();
                return;
            }

            UserSession.Start(rezultat.Account);

            // Setăm MainWindow înainte de a închide login-ul, ca aplicația să nu se oprească
            var panouPrincipal = new MainWindow();
            Application.Current.MainWindow = panouPrincipal;
            panouPrincipal.Show();
            Close();
        }
        catch (Exception ex)
        {
            UserSession.End();
            AfiseazaEroare($"Eroare la deschiderea panoului: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void AfiseazaEroare(string mesaj)
    {
        ErrorTextBlock.Text = mesaj;
        ErrorTextBlock.Visibility = Visibility.Visible;
    }

    private void ClearError()
    {
        ErrorTextBlock.Text = string.Empty;
        ErrorTextBlock.Visibility = Visibility.Collapsed;
    }

    private void SetBusy(bool ocupat)
    {
        LoginButton.IsEnabled = !ocupat;
        LoginTextBox.IsEnabled = !ocupat;
        PasswordBox.IsEnabled = !ocupat;
        LoginButton.Content = ocupat ? "Se conectează..." : "Autentificare";
    }
}
