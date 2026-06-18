using System.Windows;
using EngineeringManagementApp.Enums;
using Microsoft.Win32;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Acțiuni comune de import/export Excel pentru paginile CRUD.
/// </summary>
public static class ExcelPageActions
{
    public static async Task ExportaAsync(Window? owner, PermissionService.Table tabel)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Export Excel",
            Filter = "Fișiere Excel (*.xlsx)|*.xlsx",
            FileName = $"{NumeFisier(tabel)}_{DateTime.Now:yyyy-MM-dd}.xlsx",
            DefaultExt = ".xlsx"
        };

        if (dialog.ShowDialog(owner) != true)
            return;

        try
        {
            var serviciu = new ExcelService();
            await serviciu.ExportaAsync(tabel, dialog.FileName);
            MessageBox.Show(owner, $"Datele au fost exportate în:\n{dialog.FileName}", "Export reușit",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(owner, ex.Message, "Eroare export", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    public static async Task<bool> ImportaAsync(Window? owner, PermissionService.Table tabel, Func<Task> reincarcaDate)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Import Excel",
            Filter = "Fișiere Excel (*.xlsx)|*.xlsx",
            DefaultExt = ".xlsx"
        };

        if (dialog.ShowDialog(owner) != true)
            return false;

        if (MessageBox.Show(owner,
                "Importul va adăuga rânduri noi sau va actualiza rândurile existente după coloana ID.\nContinuați?",
                "Confirmare import",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) != MessageBoxResult.Yes)
            return false;

        try
        {
            var serviciu = new ExcelService();
            var rezultat = await serviciu.ImportaAsync(tabel, dialog.FileName);
            await reincarcaDate();

            var mesaj = $"Inserate: {rezultat.Inserate}\nActualizate: {rezultat.Actualizate}\nSărite: {rezultat.Sarite}";
            if (rezultat.Erori.Count > 0)
            {
                mesaj += $"\n\nErori ({rezultat.Erori.Count}):\n" + string.Join("\n", rezultat.Erori.Take(10));
                if (rezultat.Erori.Count > 10)
                    mesaj += $"\n... și încă {rezultat.Erori.Count - 10} erori.";
            }

            MessageBox.Show(owner, mesaj,
                rezultat.Erori.Count == 0 ? "Import reușit" : "Import finalizat cu erori",
                MessageBoxButton.OK,
                rezultat.Erori.Count == 0 ? MessageBoxImage.Information : MessageBoxImage.Warning);

            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(owner, ex.Message, "Eroare import", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
    }

    public static Visibility VizibilitateImportExport(TipDrepturi rol, PermissionService.Table tabel) =>
        PermissionService.Can(rol, tabel, PermissionService.Action.ImportExport)
            ? Visibility.Visible
            : Visibility.Collapsed;

    private static string NumeFisier(PermissionService.Table tabel) => tabel switch
    {
        PermissionService.Table.Solicitari => "solicitari",
        PermissionService.Table.Ingineri => "ingineri",
        PermissionService.Table.Echipamente => "echipamente",
        _ => "date"
    };
}
