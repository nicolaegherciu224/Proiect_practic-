using EngineeringManagementApp.Models;
using EngineeringManagementApp.Security;
using Microsoft.EntityFrameworkCore;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Operații CRUD pentru tabelul Tabel_Conturi.
/// </summary>
public sealed class ConturiService
{
    public async Task<List<Cont>> ObtineToateAsync()
    {
        await using var ctx = AppServices.CreazaContext();
        return await ctx.Conturi.AsNoTracking().OrderBy(c => c.Login).ToListAsync();
    }

    public async Task AdaugaAsync(Cont cont, string parolaClar)
    {
        Valideaza(cont, parolaClar, esteEditare: false);
        cont.Parola = PasswordHasher.Hash(parolaClar);

        await using var ctx = AppServices.CreazaContext();
        if (await ctx.Conturi.AnyAsync(c => c.Login == cont.Login))
            throw new ArgumentException($"Login-ul '{cont.Login}' există deja.");

        ctx.Conturi.Add(cont);
        await ctx.SaveChangesAsync();
    }

    public async Task ActualizeazaAsync(Cont cont, string? parolaClar)
    {
        Valideaza(cont, parolaClar, esteEditare: true);

        await using var ctx = AppServices.CreazaContext();
        var existent = await ctx.Conturi.FindAsync(cont.IdAccount)
            ?? throw new InvalidOperationException("Contul nu a fost găsit.");

        if (await ctx.Conturi.AnyAsync(c => c.Login == cont.Login && c.IdAccount != cont.IdAccount))
            throw new ArgumentException($"Login-ul '{cont.Login}' există deja.");

        existent.Login = cont.Login.Trim();
        existent.TipDrepturi = cont.TipDrepturi;

        // Parola se schimbă doar dacă utilizatorul a introdus una nouă
        if (!string.IsNullOrWhiteSpace(parolaClar))
            existent.Parola = PasswordHasher.Hash(parolaClar);

        await ctx.SaveChangesAsync();
    }

    public async Task StergeAsync(int id)
    {
        await using var ctx = AppServices.CreazaContext();
        var entitate = await ctx.Conturi.FindAsync(id);
        if (entitate is null)
            throw new InvalidOperationException("Contul nu a fost găsit.");

        ctx.Conturi.Remove(entitate);
        await ctx.SaveChangesAsync();
    }

    /// <summary>
    /// Resetează parola unui cont existent (utilitar CLI când parola a fost uitată).
    /// </summary>
    public async Task ReseteazaParolaAsync(string login, string parolaNoua)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login-ul este obligatoriu.");
        if (string.IsNullOrWhiteSpace(parolaNoua))
            throw new ArgumentException("Parola nouă este obligatorie.");

        await using var ctx = AppServices.CreazaContext();
        var cont = await ctx.Conturi.FirstOrDefaultAsync(c => c.Login == login.Trim())
            ?? throw new InvalidOperationException($"Contul '{login}' nu există.");

        cont.Parola = PasswordHasher.Hash(parolaNoua);
        await ctx.SaveChangesAsync();
    }

    public static void Valideaza(Cont c, string? parola, bool esteEditare)
    {
        if (string.IsNullOrWhiteSpace(c.Login))
            throw new ArgumentException("Login-ul este obligatoriu.");
        if (string.IsNullOrWhiteSpace(c.TipDrepturi))
            throw new ArgumentException("Tipul de drepturi este obligatoriu.");
        if (!esteEditare && string.IsNullOrWhiteSpace(parola))
            throw new ArgumentException("Parola este obligatorie la crearea contului.");
    }
}
