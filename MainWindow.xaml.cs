using System.Windows;
using System.Windows.Controls;
using EngineeringManagementApp.Services;
using EngineeringManagementApp.Views;

namespace EngineeringManagementApp;

/// <summary>
/// Panoul principal al aplicației: meniu lateral, bară de sus și zonă de conținut.
/// </summary>
public partial class MainWindow : Window
{
    private NavigationService _navigare = null!;
    private readonly Dictionary<Button, PaginaNavigare> _mapareButoane = new();

    public MainWindow()
    {
        InitializeComponent();

        // Verificăm că utilizatorul este autentificat
        var sesiune = UserSession.Current;
        if (sesiune is null)
        {
            MessageBox.Show("Sesiune invalidă. Vă rugăm să vă autentificați din nou.",
                "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
            DeschideLoginSiInchide();
            return;
        }

        // Afișăm datele utilizatorului în sidebar și în titlul ferestrei
        SidebarUserText.Text = sesiune.Login;
        SidebarRolText.Text = PermissionService.GetRoleDisplayName(sesiune.Role);
        Title = $"Engineering Management — {sesiune.Login}";

        // Inițializăm serviciul de navigare și meniul conform drepturilor (RBAC)
        _navigare = new NavigationService(ZonaContinut);
        ConfigureazaMeniulDupaRol(sesiune.Role);
        ActualizeazaEtichetaTema();

        // Ascultăm schimbarea temei pentru a actualiza textul butonului
        ThemeService.TemaSchimbata += ActualizeazaEtichetaTema;
        Closed += (_, _) => ThemeService.TemaSchimbata -= ActualizeazaEtichetaTema;

        // Deschidem pagina Acasă la pornire
        SelecteazaPagina(PaginaNavigare.Acasa, BtnAcasa);
    }

    /// <summary>
    /// Ascunde din meniu secțiunile la care utilizatorul nu are acces.
    /// </summary>
    private void ConfigureazaMeniulDupaRol(Enums.TipDrepturi rol)
    {
        _mapareButoane.Clear();

        // Acasă este vizibilă pentru toți
        _mapareButoane[BtnAcasa] = PaginaNavigare.Acasa;

        // Solicitări
        BtnSolicitari.Visibility = VizibilitateDaca(PermissionService.CanViewTable(rol, PermissionService.Table.Solicitari));
        if (BtnSolicitari.Visibility == Visibility.Visible)
            _mapareButoane[BtnSolicitari] = PaginaNavigare.Solicitari;

        // Ingineri
        BtnIngineri.Visibility = VizibilitateDaca(PermissionService.CanViewTable(rol, PermissionService.Table.Ingineri));
        if (BtnIngineri.Visibility == Visibility.Visible)
            _mapareButoane[BtnIngineri] = PaginaNavigare.Ingineri;

        // Echipamente
        BtnEchipamente.Visibility = VizibilitateDaca(PermissionService.CanViewTable(rol, PermissionService.Table.Echipamente));
        if (BtnEchipamente.Visibility == Visibility.Visible)
            _mapareButoane[BtnEchipamente] = PaginaNavigare.Echipamente;

        // Statistici — Admin și Super Admin
        BtnStatistici.Visibility = VizibilitateDaca(PermissionService.CanViewStatistici(rol));
        if (BtnStatistici.Visibility == Visibility.Visible)
            _mapareButoane[BtnStatistici] = PaginaNavigare.Statistici;

        // Conturi — doar Super Admin
        BtnConturi.Visibility = VizibilitateDaca(PermissionService.CanViewTable(rol, PermissionService.Table.Conturi));
        if (BtnConturi.Visibility == Visibility.Visible)
            _mapareButoane[BtnConturi] = PaginaNavigare.Conturi;
    }

    private static Visibility VizibilitateDaca(bool areAcces) =>
        areAcces ? Visibility.Visible : Visibility.Collapsed;

    private void Navigare_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button buton)
            return;

        if (!_mapareButoane.TryGetValue(buton, out var pagina))
            return;

        SelecteazaPagina(pagina, buton);
    }

    /// <summary>
    /// Încarcă pagina și marchează butonul activ din meniu.
    /// </summary>
    private void SelecteazaPagina(PaginaNavigare pagina, Button butonActiv)
    {
        _navigare.Navigheaza(pagina);
        TitluPaginaText.Text = ObtineTitluPagina(pagina);

        // Resetăm starea „activ” pe toate butoanele din meniu
        foreach (var buton in _mapareButoane.Keys)
        {
            buton.Tag = buton == butonActiv ? "Active" : null;
        }
    }

    private static string ObtineTitluPagina(PaginaNavigare pagina) => pagina switch
    {
        PaginaNavigare.Acasa => "Acasă",
        PaginaNavigare.Solicitari => "Solicitări",
        PaginaNavigare.Ingineri => "Ingineri",
        PaginaNavigare.Echipamente => "Echipamente",
        PaginaNavigare.Statistici => "Statistici",
        PaginaNavigare.Conturi => "Conturi utilizatori",
        _ => "Engineering Management"
    };

    private void ComutaTema_Click(object sender, RoutedEventArgs e)
    {
        ThemeService.ComutaTema();
    }

    private void ActualizeazaEtichetaTema()
    {
        BtnComutaTema.Content = ThemeService.EtichetaButonTema;
    }

    private void Deconectare_Click(object sender, RoutedEventArgs e)
    {
        var confirmare = MessageBox.Show(
            "Sigur doriți să vă deconectați?",
            "Deconectare",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirmare == MessageBoxResult.Yes)
        {
            UserSession.End();
            DeschideLoginSiInchide();
        }
    }

    /// <summary>
    /// Închide panoul principal și redeschide fereastra de login.
    /// </summary>
    private void DeschideLoginSiInchide()
    {
        var login = new LoginWindow();
        Application.Current.MainWindow = login;
        login.Show();
        Close();
    }
}
