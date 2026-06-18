using EngineeringManagementApp.Data;
using EngineeringManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Înregistrează și citește istoricul schimbărilor de status pentru solicitări.
/// </summary>
public sealed class SolicitareAuditService
{
    public async Task<List<SolicitareAudit>> ObtineIstoricAsync(int idSolicitare)
    {
        await using var ctx = AppServices.CreazaContext();
        return await ctx.SolicitariAudit.AsNoTracking()
            .Where(a => a.IdSolicitare == idSolicitare)
            .OrderByDescending(a => a.DataSchimbare)
            .ToListAsync();
    }

    public static void Inregistreaza(
        AppDbContext ctx,
        int idSolicitare,
        string statusVechi,
        string statusNou,
        string? observatii = null)
    {
        if (string.Equals(statusVechi, statusNou, StringComparison.Ordinal))
            return;

        var utilizator = UserSession.Current?.Login ?? "sistem";

        ctx.SolicitariAudit.Add(new SolicitareAudit
        {
            IdSolicitare = idSolicitare,
            StatusVechi = statusVechi,
            StatusNou = statusNou,
            DataSchimbare = DateTime.Now,
            Utilizator = utilizator,
            Observatii = observatii
        });
    }
}
