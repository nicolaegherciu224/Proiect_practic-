using System.Windows;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;

namespace EngineeringManagementApp.Views.Dialogs;

public partial class SolicitareAuditDialog : Window
{
    public SolicitareAuditDialog(Solicitare solicitare)
    {
        InitializeComponent();
        TitluText.Text = $"Solicitare #{solicitare.IdSolicitare} — {solicitare.Client}";
        _ = IncarcaIstoricAsync(solicitare.IdSolicitare);
    }

    private async Task IncarcaIstoricAsync(int idSolicitare)
    {
        try
        {
            var serviciu = new SolicitareAuditService();
            GridIstoric.ItemsSource = await serviciu.ObtineIstoricAsync(idSolicitare);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la încărcarea istoricului: {ex.Message}", "Eroare",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Inchide_Click(object sender, RoutedEventArgs e) => Close();
}
