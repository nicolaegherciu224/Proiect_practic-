using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

public sealed class GrupEchipamentStatistici
{
    public string TipEchipament { get; init; } = string.Empty;
    public int Cantitate { get; init; }
}

public sealed class VenitLunarStatistici
{
    public string Luna { get; init; } = string.Empty;
    public decimal Venit { get; init; }
}

public sealed class TopInginerStatistici
{
    public string NumeComplet { get; init; } = string.Empty;
    public int EchipamenteReparate { get; init; }
}

/// <summary>
/// Agregări pentru dashboard-ul de statistici.
/// </summary>
public sealed class StatisticiService
{
    public async Task<List<GrupEchipamentStatistici>> ObtineDistributieEchipamenteAsync()
    {
        await using var ctx = AppServices.CreazaContext();

        return await ctx.Solicitari.AsNoTracking()
            .Where(s => s.Status != "Anulat")
            .GroupBy(s => s.TipEchipament)
            .Select(g => new GrupEchipamentStatistici
            {
                TipEchipament = g.Key,
                Cantitate = g.Sum(s => s.CantitateEchipament)
            })
            .OrderByDescending(g => g.Cantitate)
            .ToListAsync();
    }

    public async Task<List<VenitLunarStatistici>> ObtineVenituriLunareAsync(int luni = 6)
    {
        await using var ctx = AppServices.CreazaContext();
        var deLa = DateTime.Today.AddMonths(-luni + 1);
        deLa = new DateTime(deLa.Year, deLa.Month, 1);

        var solicitari = await ctx.Solicitari.AsNoTracking()
            .Where(s => s.Data >= deLa && s.Status == "Finalizat")
            .ToListAsync();

        return solicitari
            .GroupBy(s => new { s.Data.Year, s.Data.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new VenitLunarStatistici
            {
                Luna = $"{g.Key.Month:00}.{g.Key.Year}",
                Venit = g.Sum(s => s.SumaAchitare)
            })
            .ToList();
    }

    public async Task<List<TopInginerStatistici>> ObtineTopIngineriAsync(int limita = 10)
    {
        await using var ctx = AppServices.CreazaContext();

        return await ctx.Solicitari.AsNoTracking()
            .Where(s => s.Status != "Anulat")
            .GroupBy(s => new { s.NumeInginer, s.PrenumeInginer })
            .Select(g => new TopInginerStatistici
            {
                NumeComplet = g.Key.NumeInginer + " " + g.Key.PrenumeInginer,
                EchipamenteReparate = g.Sum(s => s.CantitateEchipament)
            })
            .OrderByDescending(g => g.EchipamenteReparate)
            .Take(limita)
            .ToListAsync();
    }
}
