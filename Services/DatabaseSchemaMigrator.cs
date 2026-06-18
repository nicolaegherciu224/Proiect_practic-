using EngineeringManagementApp.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Aplică modificări de schemă pe baze SQLite existente (EnsureCreated nu actualizează tabele vechi).
/// </summary>
public static class DatabaseSchemaMigrator
{
    public static async Task AplicaAsync(AppDbContext ctx, CancellationToken cancellationToken = default)
    {
        var connection = ctx.Database.GetDbConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            await ExecutaDacaLipsesteColoanaAsync(connection,
                "solicitari", "status",
                "ALTER TABLE solicitari ADD COLUMN status TEXT NOT NULL DEFAULT 'Preluat';",
                cancellationToken).ConfigureAwait(false);

            await ExecutaDacaLipsesteColoanaAsync(connection,
                "solicitari", "nr_serie",
                "ALTER TABLE solicitari ADD COLUMN nr_serie TEXT;",
                cancellationToken).ConfigureAwait(false);

            await ExecutaDacaLipsesteTabelulAsync(connection,
                "solicitari_audit",
                """
                CREATE TABLE IF NOT EXISTS solicitari_audit (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    nr_solicitare INTEGER NOT NULL,
                    status_vechi TEXT,
                    status_nou TEXT NOT NULL,
                    data_schimbare TEXT NOT NULL,
                    utilizator TEXT NOT NULL,
                    observatii TEXT
                );
                """,
                cancellationToken).ConfigureAwait(false);

            await CorecteazaValoriNuleAsync(connection, cancellationToken).ConfigureAwait(false);

            await connection.CloseAsync().ConfigureAwait(false);
        }
        catch
        {
            await connection.CloseAsync().ConfigureAwait(false);
            throw;
        }
    }

    private static async Task ExecutaDacaLipsesteColoanaAsync(
        System.Data.Common.DbConnection connection,
        string tabel,
        string coloana,
        string sql,
        CancellationToken cancellationToken)
    {
        if (await ColoanaExistaAsync(connection, tabel, coloana, cancellationToken).ConfigureAwait(false))
            return;

        await ExecutaSqlAsync(connection, sql, cancellationToken).ConfigureAwait(false);
    }

    private static async Task ExecutaDacaLipsesteTabelulAsync(
        System.Data.Common.DbConnection connection,
        string tabel,
        string sql,
        CancellationToken cancellationToken)
    {
        if (await TabelExistaAsync(connection, tabel, cancellationToken).ConfigureAwait(false))
            return;

        await ExecutaSqlAsync(connection, sql, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<bool> ColoanaExistaAsync(
        System.Data.Common.DbConnection connection,
        string tabel,
        string coloana,
        CancellationToken cancellationToken)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = $"PRAGMA table_info({tabel});";

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var nume = reader.GetString(1);
            if (string.Equals(nume, coloana, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private static async Task<bool> TabelExistaAsync(
        System.Data.Common.DbConnection connection,
        string tabel,
        CancellationToken cancellationToken)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=$tabel;";
        cmd.Parameters.Add(new SqliteParameter("$tabel", tabel));
        var rezultat = await cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return rezultat is not null;
    }

    private static async Task ExecutaSqlAsync(
        System.Data.Common.DbConnection connection,
        string sql,
        CancellationToken cancellationToken)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        await cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Înlocuiește NULL-urile din baze existente — EF nu poate citi NULL în proprietăți non-nullable.
    /// </summary>
    private static async Task CorecteazaValoriNuleAsync(
        System.Data.Common.DbConnection connection,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE solicitari SET nume_inginer = '' WHERE nume_inginer IS NULL;
            UPDATE solicitari SET prenume_inginer = '' WHERE prenume_inginer IS NULL;
            UPDATE solicitari SET client = '' WHERE client IS NULL;
            UPDATE solicitari SET adresa = '' WHERE adresa IS NULL;
            UPDATE solicitari SET data = '1970-01-01' WHERE data IS NULL;
            UPDATE solicitari SET tip_echipament = '' WHERE tip_echipament IS NULL;
            UPDATE solicitari SET model_echipament = '' WHERE model_echipament IS NULL;
            UPDATE solicitari SET nr_serie = '' WHERE nr_serie IS NULL;
            UPDATE solicitari SET cantitate = 0 WHERE cantitate IS NULL;
            UPDATE solicitari SET suma = 0 WHERE suma IS NULL;
            UPDATE solicitari SET status = 'Preluat' WHERE status IS NULL;

            UPDATE ingineri SET nume = '' WHERE nume IS NULL;
            UPDATE ingineri SET prenume = '' WHERE prenume IS NULL;
            UPDATE ingineri SET cantitate_echipamente = 0 WHERE cantitate_echipamente IS NULL;

            UPDATE echipamente SET tip_echipament = '' WHERE tip_echipament IS NULL;
            UPDATE echipamente SET model = '' WHERE model IS NULL;
            UPDATE echipamente SET nr_serie = '' WHERE nr_serie IS NULL;

            UPDATE conturi SET login = '' WHERE login IS NULL;
            UPDATE conturi SET parola = '' WHERE parola IS NULL;
            UPDATE conturi SET tip_drepturi = '' WHERE tip_drepturi IS NULL;

            UPDATE solicitari_audit SET status_vechi = '' WHERE status_vechi IS NULL;
            UPDATE solicitari_audit SET status_nou = 'Preluat' WHERE status_nou IS NULL;
            UPDATE solicitari_audit SET data_schimbare = datetime('now') WHERE data_schimbare IS NULL;
            UPDATE solicitari_audit SET utilizator = 'sistem' WHERE utilizator IS NULL;
            """;

        foreach (var statement in sql.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            await ExecutaSqlAsync(connection, statement, cancellationToken).ConfigureAwait(false);
        }
    }
}
