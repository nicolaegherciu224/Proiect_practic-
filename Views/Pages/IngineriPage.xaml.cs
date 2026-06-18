using System.Windows;
using System.Windows.Controls;
using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;
using EngineeringManagementApp.Views.Dialogs;

namespace EngineeringManagementApp.Views.Pages;

/// <summary>
/// Pagina CRUD pentru ingineri (doar vizualizare pentru rolul Inginer).
/// </summary>
public partial class IngineriPage : Page
{
    private readonly IngineriService _serviciu = new();

    public IngineriPage()
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
        var tabel = PermissionService.Table.Ingineri;

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
            GridDate.ItemsSource = await _serviciu.ObtineTotiAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la încărcare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private Inginer? SelectieCurenta() => GridDate.SelectedItem as Inginer;

    private async void Adauga_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new InginerDialog { Owner = Window.GetWindow(this) };
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
            MessageBox.Show("Selectați un inginer din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new InginerDialog(selectie) { Owner = Window.GetWindow(this) };
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
            MessageBox.Show("Selectați un inginer din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (MessageBox.Show($"Ștergeți inginerul {selectie.NumeInginer} {selectie.PrenumeInginer}?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        try
        {
            await _serviciu.StergeAsync(selectie.IdInginer);
            await IncarcaDateAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void Reincarca_Click(object sender, RoutedEventArgs e) => await IncarcaDateAsync();

    private async void Export_Click(object sender, RoutedEventArgs e) =>
        await ExcelPageActions.ExportaAsync(Window.GetWindow(this), PermissionService.Table.Ingineri);

    private async void Import_Click(object sender, RoutedEventArgs e) =>
        await ExcelPageActions.ImportaAsync(Window.GetWindow(this), PermissionService.Table.Ingineri, IncarcaDateAsync);
}
