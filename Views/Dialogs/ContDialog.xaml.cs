using System.Windows;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;

namespace EngineeringManagementApp.Views.Dialogs;

/// <summary>
/// Formular pentru adăugare sau editare cont utilizator.
/// </summary>
public partial class ContDialog : Window
{
    private readonly Cont? _existent;
    private readonly bool _esteEditare;

    public Cont Rezultat { get; private set; } = new();
    public string? ParolaClar { get; private set; }

    public ContDialog(Cont? existent = null)
    {
        InitializeComponent();
        _existent = existent;
        _esteEditare = existent is not null;

        RolCombo.ItemsSource = new[] { "Super Admin", "Admin", "Inginer" };

        if (existent is not null)
        {
            TitluText.Text = "Editează cont";
            ParolaLabel.Text = "Parolă nouă (lăsați gol pentru a păstra parola)";
            LoginBox.Text = existent.Login;
            RolCombo.SelectedItem = existent.TipDrepturi;
        }
        else
        {
            RolCombo.SelectedIndex = 2; // Inginer implicit
        }
    }

    private void Salveaza_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Rezultat = new Cont
            {
                IdAccount = _existent?.IdAccount ?? 0,
                Login = LoginBox.Text.Trim(),
                TipDrepturi = RolCombo.SelectedItem?.ToString() ?? string.Empty,
                Parola = _existent?.Parola ?? string.Empty
            };

            ParolaClar = ParolaBox.Password;
            ConturiService.Valideaza(Rezultat, ParolaClar, _esteEditare);

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            EroareText.Text = ex.Message;
            EroareText.Visibility = Visibility.Visible;
        }
    }

    private void Anuleaza_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
