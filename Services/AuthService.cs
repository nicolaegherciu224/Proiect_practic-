using EngineeringManagementApp.Data;
using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;
using EngineeringManagementApp.Security;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Result of an authentication attempt.
/// </summary>
public sealed class AuthResult
{
  public bool Success { get; init; }
  public string Message { get; init; } = string.Empty;
  public Cont? Account { get; init; }

  public static AuthResult Ok(Cont account) => new() { Success = true, Account = account };

  public static AuthResult Fail(string message) => new() { Success = false, Message = message };
}

/// <summary>
/// Handles user login validation against Tabel_Conturi.
/// </summary>
public sealed class AuthService
{
  private readonly AppDbContext _dbContext;

  public AuthService(AppDbContext dbContext)
  {
    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
  }

  /// <summary>
  /// Validates credentials and returns the matching account when successful.
  /// </summary>
  public async Task<AuthResult> LoginAsync(string login, string password, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(login))
    {
      return AuthResult.Fail("Introduceți numele de utilizator.");
    }

    if (string.IsNullOrWhiteSpace(password))
    {
      return AuthResult.Fail("Introduceți parola.");
    }

    var normalizedLogin = login.Trim();

    Cont? account;
    try
    {
      account = await _dbContext.Conturi
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Login == normalizedLogin, cancellationToken)
        .ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      return AuthResult.Fail($"Eroare la conectarea la baza de date: {ex.Message}");
    }

    if (account is null)
    {
      return AuthResult.Fail("Utilizator sau parolă incorectă.");
    }

    if (!PasswordHasher.Verify(password, account.Parola))
    {
      return AuthResult.Fail("Utilizator sau parolă incorectă.");
    }

    return AuthResult.Ok(account);
  }
}
