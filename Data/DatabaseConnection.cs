using System.IO;
using Microsoft.Extensions.Configuration;

namespace EngineeringManagementApp.Data;

/// <summary>
/// Gestionează conexiunea la baza de date SQLite locală (fișier .db).
/// Nu necesită server — baza rulează direct cu aplicația .exe.
/// </summary>
public sealed class DatabaseConnection
{
    private readonly IConfiguration _configuration;

    public DatabaseConnection(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Calea absolută către fișierul monitorizare.db.
  /// Implicit: Database/monitorizare.db lângă executabil.
    /// </summary>
    public string CaleFisierBazaDate
    {
        get
        {
            var caleRelativa = _configuration.GetValue<string>("Database:FilePath") ?? "Database/monitorizare.db";
            return Path.IsPathRooted(caleRelativa)
                ? caleRelativa
                : Path.Combine(AppContext.BaseDirectory, caleRelativa);
        }
    }

    /// <summary>
    /// Connection string SQLite pentru Entity Framework.
    /// </summary>
    public string ConnectionString => $"Data Source={CaleFisierBazaDate}";

    public int CommandTimeoutSeconds =>
        _configuration.GetValue("Database:CommandTimeoutSeconds", 30);

    /// <summary>
    /// Asigură că folderul pentru fișierul .db există.
    /// </summary>
    public void AsiguraDirectorul()
    {
        var director = Path.GetDirectoryName(CaleFisierBazaDate);
        if (!string.IsNullOrEmpty(director))
        {
            Directory.CreateDirectory(director);
        }
    }

    /// <summary>
    /// Verifică dacă fișierul bazei de date există și poate fi deschis.
    /// </summary>
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        AsiguraDirectorul();
        await using var connection = new Microsoft.Data.Sqlite.SqliteConnection(ConnectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        return connection.State == System.Data.ConnectionState.Open;
    }
}
