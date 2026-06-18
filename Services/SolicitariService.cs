using EngineeringManagementApp.Models;
using EngineeringManagementApp.Enums;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Operații CRUD pentru tabelul solicitari, cu audit la schimbarea statusului.
/// </summary>
public sealed class SolicitariService
{
    public async Task<List<Solicitare>> ObtineToateAsync()
    {
        await using var ctx = AppServices.CreazaContext();
        return await ctx.Solicitari.AsNoTracking().OrderByDescending(s => s.Data).ToListAsync();
    }

    public async Task AdaugaAsync(Solicitare solicitare)
    {
        Valideaza(solicitare);

        if (string.IsNullOrWhiteSpace(solicitare.Status))
            solicitare.Status = StatusSolicitareMapper.ToStorage(StatusSolicitare.Preluat);

        await using var ctx = AppServices.CreazaContext();
        ctx.Solicitari.Add(solicitare);
        await ctx.SaveChangesAsync();

        // Înregistrează crearea în audit după ce ID-ul a fost generat
        if (solicitare.IdSolicitare > 0)
        {
            SolicitareAuditService.Inregistreaza(ctx, solicitare.IdSolicitare, "—", solicitare.Status, "Solicitare creată");
            await ctx.SaveChangesAsync();
        }
    }

    public async Task ActualizeazaAsync(Solicitare solicitare)
    {
        Valideaza(solicitare);

        await using var ctx = AppServices.CreazaContext();
        var existent = await ctx.Solicitari.FindAsync(solicitare.IdSolicitare)
            ?? throw new InvalidOperationException("Solicitarea nu a fost găsită.");

        SolicitareAuditService.Inregistreaza(ctx, existent.IdSolicitare, existent.Status, solicitare.Status);

        existent.NumeInginer = solicitare.NumeInginer;
        existent.PrenumeInginer = solicitare.PrenumeInginer;
        existent.Client = solicitare.Client;
        existent.Adresa = solicitare.Adresa;
        existent.Data = solicitare.Data;
        existent.TipEchipament = solicitare.TipEchipament;
        existent.ModelEchipament = solicitare.ModelEchipament;
        existent.NrSerie = solicitare.NrSerie;
        existent.CantitateEchipament = solicitare.CantitateEchipament;
        existent.SumaAchitare = solicitare.SumaAchitare;
        existent.Status = solicitare.Status;

        await ctx.SaveChangesAsync();
    }

    public async Task StergeAsync(int id)
    {
        await using var ctx = AppServices.CreazaContext();
        var entitate = await ctx.Solicitari.FindAsync(id);
        if (entitate is null)
            throw new InvalidOperationException("Solicitarea nu a fost găsită.");

        ctx.Solicitari.Remove(entitate);
        await ctx.SaveChangesAsync();
    }

    public static void Valideaza(Solicitare s)
    {
        if (string.IsNullOrWhiteSpace(s.NumeInginer))
            throw new ArgumentException("Numele inginerului este obligatoriu.");
        if (string.IsNullOrWhiteSpace(s.PrenumeInginer))
            throw new ArgumentException("Prenumele inginerului este obligatoriu.");
        if (string.IsNullOrWhiteSpace(s.Client))
            throw new ArgumentException("Clientul este obligatoriu.");
        if (string.IsNullOrWhiteSpace(s.Adresa))
            throw new ArgumentException("Adresa este obligatorie.");
        if (string.IsNullOrWhiteSpace(s.TipEchipament))
            throw new ArgumentException("Tipul echipamentului este obligatoriu.");
        if (string.IsNullOrWhiteSpace(s.ModelEchipament))
            throw new ArgumentException("Modelul echipamentului este obligatoriu.");
        if (s.CantitateEchipament < 0)
            throw new ArgumentException("Cantitatea nu poate fi negativă.");
        if (s.SumaAchitare < 0)
            throw new ArgumentException("Suma de achitare nu poate fi negativă.");
    }
}
