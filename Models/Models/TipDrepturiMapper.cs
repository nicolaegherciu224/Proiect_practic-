using EngineeringManagementApp.Enums;

namespace EngineeringManagementApp.Models;

/// <summary>
/// Maps between database string values and <see cref="TipDrepturi"/> enum.
/// </summary>
public static class TipDrepturiMapper
{
  public static string ToStorage(TipDrepturi rol) => rol switch
  {
    TipDrepturi.SuperAdmin => "Super Admin",
    TipDrepturi.Admin => "Admin",
    TipDrepturi.Inginer => "Inginer",
    _ => throw new ArgumentOutOfRangeException(nameof(rol), rol, "Unknown role.")
  };

  public static TipDrepturi FromStorage(string value) => value switch
  {
    "Super Admin" => TipDrepturi.SuperAdmin,
    "Admin" => TipDrepturi.Admin,
    "Inginer" => TipDrepturi.Inginer,
    _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown role stored in database.")
  };
}
