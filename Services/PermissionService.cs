using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Definește ce poate face fiecare rol pentru fiecare tabel.
/// </summary>
public static class PermissionService
{
  public enum Table
  {
    Solicitari,
    Ingineri,
    Echipamente,
    Conturi
  }

  public enum Action
  {
    View,
    Create,
    Update,
    Delete,
    ImportExport
  }

  public static bool Can(TipDrepturi role, Table table, Action action) => (role, table, action) switch
  {
    // Super Admin: acces complet la toate tabelele
    (TipDrepturi.SuperAdmin, _, _) => true,

    // Admin: tot, exceptând Conturi
    (TipDrepturi.Admin, Table.Conturi, _) => false,
    (TipDrepturi.Admin, _, _) => true,

    // Inginer: Solicitări (vizualizare/adăugare/editare), Ingineri (doar vizualizare)
    (TipDrepturi.Inginer, Table.Solicitari, Action.View) => true,
    (TipDrepturi.Inginer, Table.Solicitari, Action.Create) => true,
    (TipDrepturi.Inginer, Table.Solicitari, Action.Update) => true,
    (TipDrepturi.Inginer, Table.Solicitari, _) => false,
    (TipDrepturi.Inginer, Table.Ingineri, Action.View) => true,
    (TipDrepturi.Inginer, Table.Ingineri, _) => false,
    (TipDrepturi.Inginer, _, _) => false,

    _ => false
  };

  public static bool CanViewTable(TipDrepturi role, Table table) => Can(role, table, Action.View);

  public static bool CanViewStatistici(TipDrepturi role) =>
    role is TipDrepturi.SuperAdmin or TipDrepturi.Admin;

  public static string GetRoleDisplayName(TipDrepturi role) => TipDrepturiMapper.ToStorage(role);
}
