using System.Security.Cryptography;
using System.Text;

namespace EngineeringManagementApp.Security;

/// <summary>
/// Verificare parole: SHA-256 (baza existentă) și BCrypt (conturi noi).
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Generează hash SHA-256 (formatul din baza monitorizare.sql).
    /// </summary>
    public static string Hash(string parolaClar)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parolaClar);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(parolaClar));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

  /// <summary>
    /// Verifică parola față de hash SHA-256 sau BCrypt.
    /// </summary>
    public static bool Verify(string parolaClar, string hashSalvat)
    {
        if (string.IsNullOrWhiteSpace(parolaClar) || string.IsNullOrWhiteSpace(hashSalvat))
            return false;

        // Hash SHA-256 din monitorizare.sql (64 caractere hex)
        if (hashSalvat.Length == 64 && hashSalvat.All(Uri.IsHexDigit))
            return Hash(parolaClar) == hashSalvat.ToLowerInvariant();

        // Conturi noi create cu BCrypt
        try
        {
            return BCrypt.Net.BCrypt.Verify(parolaClar, hashSalvat);
        }
        catch
        {
            return false;
        }
    }
}
