using System.Windows;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;

namespace EngineeringManagementApp.Views.Dialogs;

/// <summary>
/// Formular pentru adăugare sau editare inginer.
/// </summary>
public partial class InginerDialog : Window
{
    private readonly Inginer? _existent;

    public Inginer Rezultat { get; private set; } = new();

    public InginerDialog(Inginer? existent = null)
    {
        InitializeComponent();
        _existent = existent;

        if (existent is not null)
        {
            TitluText.Text = "Editează inginer";
            NumeBox.Text = existent.NumeInginer;
            PrenumeBox.Text = existent.PrenumeInginer;
            CantitateBox.Text = existent.CantitateEchipamenteDeservite.ToString();
        }
        else
        {
            CantitateBox.Text = "0";
        }
    }

    private void Salveaza_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!int.TryParse(CantitateBox.Text, out var cantitate))
                throw new ArgumentException("Cantitatea trebuie să fie un număr întreg.");

            Rezultat = new Inginer
            {
                IdInginer = _existent?.IdInginer ?? 0,
                NumeInginer = NumeBox.Text.Trim(),
                PrenumeInginer = PrenumeBox.Text.Trim(),
                CantitateEchipamenteDeservite = cantitate
            };

            IngineriService.Valideaza(Rezultat);
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
