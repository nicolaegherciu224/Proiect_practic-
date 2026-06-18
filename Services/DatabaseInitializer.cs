using System;
using System.IO;
using EngineeringManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Creează fișierul SQLite și încarcă datele din monitorizare.sql la prima pornire.
/// </summary>
public sealed class DatabaseInitializer
{
    private readonly AppDbContext _dbContext;
    private readonly DatabaseConnection _databaseConnection;

    public DatabaseInitializer(AppDbContext dbContext, DatabaseConnection databaseConnection)
    {
        _dbContext = dbContext;
        _databaseConnection = databaseConnection;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        // Asigură folderul Database/ lângă .exe
        _databaseConnection.AsiguraDirectorul();

        // Creează tabelele SQLite dacă nu există
        await _dbContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);

        // Actualizează schema bazelor existente (coloane noi, tabele audit)
        await DatabaseSchemaMigrator.AplicaAsync(_dbContext, cancellationToken).ConfigureAwait(false);

        // Încarcă datele din monitorizare.sql dacă baza e goală (sau dacă tabelele sunt goale)
        var trebuieSeed = !await _dbContext.Conturi.AnyAsync(cancellationToken).ConfigureAwait(false)
                         || !await _dbContext.Ingineri.AnyAsync(cancellationToken).ConfigureAwait(false)
                         || !await _dbContext.Echipamente.AnyAsync(cancellationToken).ConfigureAwait(false);

        if (trebuieSeed)
        {
            await IncarcaDateInitialeAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task IncarcaDateInitialeAsync(CancellationToken cancellationToken)
    {
        // Adaugăm doar entitățile lipsă pentru a evita duplicarea sau suprascrierea datelor
        await using var tran = await _dbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            // Conturi (unic după Login)
            foreach (var cont in MonitorizareSeedData.Conturi)
            {
                var exista = await _dbContext.Conturi.AnyAsync(c => c.Login == cont.Login, cancellationToken).ConfigureAwait(false);
                if (!exista)
                    _dbContext.Conturi.Add(cont);
            }
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Echipamente (unic după NrSerie)
            foreach (var ech in MonitorizareSeedData.Echipamente)
            {
                var exista = await _dbContext.Echipamente.AnyAsync(e => e.NrSerie == ech.NrSerie, cancellationToken).ConfigureAwait(false);
                if (!exista)
                    _dbContext.Echipamente.Add(ech);
            }
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Ingineri (unic după nume+prenume)
            foreach (var ing in MonitorizareSeedData.Ingineri)
            {
                var exista = await _dbContext.Ingineri.AnyAsync(i => i.NumeInginer == ing.NumeInginer && i.PrenumeInginer == ing.PrenumeInginer, cancellationToken).ConfigureAwait(false);
                if (!exista)
                    _dbContext.Ingineri.Add(ing);
            }
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Solicitări (unic după IdSolicitare)
            foreach (var s in MonitorizareSeedData.Solicitari)
            {
                var exista = await _dbContext.Solicitari.AnyAsync(x => x.IdSolicitare == s.IdSolicitare, cancellationToken).ConfigureAwait(false);
                if (!exista)
                    _dbContext.Solicitari.Add(s);
            }
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            await tran.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await tran.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }
}
