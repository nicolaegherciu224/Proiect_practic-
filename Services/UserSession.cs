using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Holds the authenticated user for the current application session.
/// </summary>
public sealed class UserSession
{
  public int AccountId { get; init; }
  public string Login { get; init; } = string.Empty;
  public TipDrepturi Role { get; init; }

  public string DisplayName => Login;

  public static UserSession? Current { get; private set; }

  public static void Start(Cont account)
  {
    ArgumentNullException.ThrowIfNull(account);

    Current = new UserSession
    {
      AccountId = account.IdAccount,
      Login = account.Login,
      Role = account.Rol
    };
  }

  public static void End()
  {
    Current = null;
  }

  public static bool IsAuthenticated => Current is not null;
}
