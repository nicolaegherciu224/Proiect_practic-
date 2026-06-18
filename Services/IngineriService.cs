using EngineeringManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Operații CRUD pentru tabelul Tabel_Ingineri.
/// </summary>
public sealed class IngineriService
{
    public async Task<List<Inginer>> ObtineTotiAsync()
    {
        await using var ctx = AppServices.CreazaContext();
        return await ctx.Ingineri.AsNoTracking().OrderBy(i => i.NumeInginer).ToListAsync();
    }

    public async Task AdaugaAsync(Inginer inginer)
    {
        Valideaza(inginer);
        await using var ctx = AppServices.CreazaContext();
        ctx.Ingineri.Add(inginer);
        await ctx.SaveChangesAsync();
    }

    public async Task ActualizeazaAsync(Inginer inginer)
    {
        Valideaza(inginer);
        await using var ctx = AppServices.CreazaContext();
        ctx.Ingineri.Update(inginer);
        await ctx.SaveChangesAsync();
    }

    public async Task StergeAsync(int id)
    {
        await using var ctx = AppServices.CreazaContext();
        var entitate = await ctx.Ingineri.FindAsync(id);
        if (entitate is null)
            throw new InvalidOperationException("Inginerul nu a fost găsit.");

        ctx.Ingineri.Remove(entitate);
        await ctx.SaveChangesAsync();
    }

    public static void Valideaza(Inginer i)
    {
        if (string.IsNullOrWhiteSpace(i.NumeInginer))
            throw new ArgumentException("Numele este obligatoriu.");
        if (string.IsNullOrWhiteSpace(i.PrenumeInginer))
            throw new ArgumentException("Prenumele este obligatoriu.");
        if (i.CantitateEchipamenteDeservite < 0)
            throw new ArgumentException("Cantitatea echipamentelor nu poate fi negativă.");
    }
}
