using System.Windows.Controls;
using EngineeringManagementApp.Services;

namespace EngineeringManagementApp.Views.Pages;

/// <summary>
/// Pagina de start după autentificare.
/// </summary>
public partial class AcasaPage : Page
{
    public AcasaPage()
    {
        InitializeComponent();

        var sesiune = UserSession.Current;
        if (sesiune is not null)
        {
            InfoUtilizatorText.Text = $"Utilizator conectat: {sesiune.Login}";
            InfoRolText.Text = $"Rol: {PermissionService.GetRoleDisplayName(sesiune.Role)}";
        }
    }
}
