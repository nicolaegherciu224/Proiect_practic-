using System.Windows;
using System.Windows.Controls;
using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Services;
using EngineeringManagementApp.Views.Dialogs;

namespace EngineeringManagementApp.Views.Pages;

/// <summary>
/// Pagina CRUD pentru conturi utilizatori.
/// </summary>
public partial class ConturiPage : Page
{
    private readonly ConturiService _serviciu = new();

    public ConturiPage()
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
        var tabel = PermissionService.Table.Conturi;

        BtnAdauga.Visibility = Viz(PermissionService.Can(rol, tabel, PermissionService.Action.Create));
        BtnEditeaza.Visibility = Viz(PermissionService.Can(rol, tabel, PermissionService.Action.Update));
        BtnSterge.Visibility = Viz(PermissionService.Can(rol, tabel, PermissionService.Action.Delete));
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

    private Cont? SelectieCurenta() => GridDate.SelectedItem as Cont;

    private async void Adauga_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContDialog { Owner = Window.GetWindow(this) };
        if (dialog.ShowDialog() != true) return;

        try
        {
            await _serviciu.AdaugaAsync(dialog.Rezultat, dialog.ParolaClar!);
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
            MessageBox.Show("Selectați un cont din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var dialog = new ContDialog(selectie) { Owner = Window.GetWindow(this) };
        if (dialog.ShowDialog() != true) return;

        try
        {
            await _serviciu.ActualizeazaAsync(dialog.Rezultat, dialog.ParolaClar);
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
            MessageBox.Show("Selectați un cont din listă.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Nu permitem ștergerea propriului cont
        if (selectie.IdAccount == UserSession.Current?.AccountId)
        {
            MessageBox.Show("Nu vă puteți șterge propriul cont cât timp sunteți conectat.", "Atenție",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (MessageBox.Show($"Ștergeți contul '{selectie.Login}'?", "Confirmare",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;

        try
        {
            await _serviciu.StergeAsync(selectie.IdAccount);
            await IncarcaDateAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void Reincarca_Click(object sender, RoutedEventArgs e) => await IncarcaDateAsync();
}
