using System.Windows;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;

namespace EngineeringManagementApp.Views.Dialogs;

/// <summary>
/// Formular pentru adăugare sau editare echipament.
/// </summary>
public partial class EchipamentDialog : Window
{
    private readonly Echipament? _existent;

    public Echipament Rezultat { get; private set; } = new();

    public EchipamentDialog(Echipament? existent = null)
    {
        InitializeComponent();
        _existent = existent;

        if (existent is not null)
        {
            if (existent.IdEchipament > 0)
                TitluText.Text = "Editează echipament";

            TipBox.Text = existent.TipEchipament;
            ModelBox.Text = existent.ModelEchipament;
            NrSerieBox.Text = existent.NrSerie;
        }
    }

    private void Salveaza_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Rezultat = new Echipament
            {
                IdEchipament = _existent?.IdEchipament ?? 0,
                TipEchipament = TipBox.Text.Trim(),
                ModelEchipament = ModelBox.Text.Trim(),
                NrSerie = NrSerieBox.Text.Trim()
            };

            EchipamenteService.Valideaza(Rezultat);
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
