using System.Windows;
using System.Windows.Controls;
using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;
using EngineeringManagementApp.Views.Dialogs;
using Microsoft.Win32;

namespace EngineeringManagementApp.Views.Pages;

/// <summary>
/// Pagina CRUD pentru solicitări.
/// </summary>
public partial class SolicitariPage : Page
{
    private readonly SolicitariService _serviciu = new();

    public SolicitariPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ConfigureazaDrepturi();
        _ = IncarcaDateAsync();
    }

    // Afișează butoanele conform rolului utilizatorului
    private void ConfigureazaDrepturi()
    {
        var rol = UserSession.Current?.Role ?? TipDrepturi.Inginer;
        var tabel = PermissionService.Table.Solicitari;

        BtnAdauga.Visibility = Viz(PermissionService.Can(rol, tabel, PermissionService.Action.Create));
        BtnEditeaza.Visibility = Viz(PermissionService.Can(rol, tabel, PermissionService.Action.Update));
        BtnSterge.Visibility = Viz(PermissionService.Can(rol, tabel, PermissionService.Action.Delete));
        BtnExport.Visibility = ExcelPageActions.VizibilitateImportExport(rol, tabel);
        BtnImport.Visibility = ExcelPageActions.VizibilitateImportExport(rol, tabel);
    }

    private static Visibility Viz(bool vizibil) => vizibil ? Visibility.Visible : Visibility.Collapsed;

    private async Task IncarcaDateAsync()
    {
        try
        {
            GridDate.ItemsSource = await _serviciu.ObtineToateAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la încărcare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private Solicitare? SelectieCurenta() => GridDate.SelectedItem as Solicitare;

    private async void Adauga_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SolicitareDialog { Owner = Window.GetWindow(this) };
        if (dialog.ShowDialog() != true) return;

        try
        {
            await _serviciu.AdaugaAsync(dialog.Rezultat);
            await IncarcaDateAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void Editeaza_Click(object sender, RoutedEventArgs e)
    {
        var selectie = SelectieCurenta();
        if (selectie is null)
        {
            MessageBox.Show("Selectați o solicitare din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new SolicitareDialog(selectie) { Owner = Window.GetWindow(this) };
        if (dialog.ShowDialog() != true) return;

        try
        {
            await _serviciu.ActualizeazaAsync(dialog.Rezultat);
            await IncarcaDateAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void Sterge_Click(object sender, RoutedEventArgs e)
    {
        var selectie = SelectieCurenta();
        if (selectie is null)
        {
            MessageBox.Show("Selectați o solicitare din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (MessageBox.Show($"Ștergeți solicitarea #{selectie.IdSolicitare}?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        try
        {
            await _serviciu.StergeAsync(selectie.IdSolicitare);
            await IncarcaDateAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void Reincarca_Click(object sender, RoutedEventArgs e) => await IncarcaDateAsync();

    private async void Export_Click(object sender, RoutedEventArgs e) =>
        await ExcelPageActions.ExportaAsync(Window.GetWindow(this), PermissionService.Table.Solicitari);

    private async void Import_Click(object sender, RoutedEventArgs e) =>
        await ExcelPageActions.ImportaAsync(Window.GetWindow(this), PermissionService.Table.Solicitari, IncarcaDateAsync);

    private void Istoric_Click(object sender, RoutedEventArgs e)
    {
        var selectie = SelectieCurenta();
        if (selectie is null)
        {
            MessageBox.Show("Selectați o solicitare din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new SolicitareAuditDialog(selectie) { Owner = Window.GetWindow(this) };
        dialog.ShowDialog();
    }

    private void Pdf_Click(object sender, RoutedEventArgs e)
    {
        var selectie = SelectieCurenta();
        if (selectie is null)
        {
            MessageBox.Show("Selectați o solicitare pentru generarea bonului PDF.", "Atenție",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new SaveFileDialog
        {
            Title = "Salvează Bon de Deservire Tehnică",
            Filter = "Fișiere PDF (*.pdf)|*.pdf",
            FileName = $"bon_deservire_{selectie.IdSolicitare}_{DateTime.Now:yyyyMMdd}.pdf",
            DefaultExt = ".pdf"
        };

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            var serviciu = new PdfBonDeservireService();
            serviciu.Genereaza(selectie, dialog.FileName);
            MessageBox.Show($"Bonul PDF a fost generat:\n{dialog.FileName}", "Export reușit",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare generare PDF", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
