using System.Windows;
using System.Windows.Controls;
using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;
using EngineeringManagementApp.Views.Dialogs;

namespace EngineeringManagementApp.Views.Pages;

/// <summary>
/// Pagina CRUD pentru echipamente.
/// </summary>
public partial class EchipamentePage : Page
{
    private readonly EchipamenteService _serviciu = new();

    public EchipamentePage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ConfigureazaDrepturi();
        _ = IncarcaDateAsync();
    }

    private void ConfigureazaDrepturi()
    {
        var rol = UserSession.Current?.Role ?? TipDrepturi.Inginer;
        var tabel = PermissionService.Table.Echipamente;

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

    private Echipament? SelectieCurenta() => GridDate.SelectedItem as Echipament;

    private async void Adauga_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new EchipamentDialog { Owner = Window.GetWindow(this) };
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
            MessageBox.Show("Selectați un echipament din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new EchipamentDialog(selectie) { Owner = Window.GetWindow(this) };
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
            MessageBox.Show("Selectați un echipament din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (MessageBox.Show($"Ștergeți echipamentul cu seria {selectie.NrSerie}?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        try
        {
            await _serviciu.StergeAsync(selectie.IdEchipament);
            await IncarcaDateAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void Reincarca_Click(object sender, RoutedEventArgs e) => await IncarcaDateAsync();

    private async void Export_Click(object sender, RoutedEventArgs e) =>
        await ExcelPageActions.ExportaAsync(Window.GetWindow(this), PermissionService.Table.Echipamente);

    private async void Import_Click(object sender, RoutedEventArgs e) =>
        await ExcelPageActions.ImportaAsync(Window.GetWindow(this), PermissionService.Table.Echipamente, IncarcaDateAsync);
}
