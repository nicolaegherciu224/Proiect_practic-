using EngineeringManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Operații CRUD pentru tabelul Tabel_Echipamente.
/// </summary>
public sealed class EchipamenteService
{
    public async Task<List<Echipament>> ObtineToateAsync()
    {
        await using var ctx = AppServices.CreazaContext();
        return await ctx.Echipamente.AsNoTracking().OrderBy(e => e.TipEchipament).ToListAsync();
    }

    public async Task<bool> ExistaNrSerieAsync(string nrSerie)
    {
        if (string.IsNullOrWhiteSpace(nrSerie))
            return false;

        await using var ctx = AppServices.CreazaContext();
        return await ctx.Echipamente.AsNoTracking()
            .AnyAsync(e => e.NrSerie == nrSerie.Trim());
    }

    public async Task AdaugaAsync(Echipament echipament)
    {
        Valideaza(echipament);
        await using var ctx = AppServices.CreazaContext();
        ctx.Echipamente.Add(echipament);
        await ctx.SaveChangesAsync();
    }

    public async Task ActualizeazaAsync(Echipament echipament)
    {
        Valideaza(echipament);
        await using var ctx = AppServices.CreazaContext();
        ctx.Echipamente.Update(echipament);
        await ctx.SaveChangesAsync();
    }

    public async Task StergeAsync(int id)
    {
        await using var ctx = AppServices.CreazaContext();
        var entitate = await ctx.Echipamente.FindAsync(id);
        if (entitate is null)
            throw new InvalidOperationException("Echipamentul nu a fost găsit.");

        ctx.Echipamente.Remove(entitate);
        await ctx.SaveChangesAsync();
    }

    public static void Valideaza(Echipament e)
    {
        if (string.IsNullOrWhiteSpace(e.TipEchipament))
            throw new ArgumentException("Tipul echipamentului este obligatoriu.");
        if (string.IsNullOrWhiteSpace(e.ModelEchipament))
            throw new ArgumentException("Modelul echipamentului este obligatoriu.");
        if (string.IsNullOrWhiteSpace(e.NrSerie))
            throw new ArgumentException("Numărul de serie este obligatoriu.");
    }
}
