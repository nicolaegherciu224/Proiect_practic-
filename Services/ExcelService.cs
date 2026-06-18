using ClosedXML.Excel;
using EngineeringManagementApp.Data;
using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace EngineeringManagementApp.Services;

public sealed class ExcelImportResult
{
    public int Inserate { get; init; }
    public int Actualizate { get; init; }
    public int Sarite { get; init; }
    public IReadOnlyList<string> Erori { get; init; } = [];
}

/// <summary>
/// Import și export date în format Excel (.xlsx) pentru solicitări, ingineri și echipamente.
/// </summary>
public sealed class ExcelService
{
    public async Task ExportaAsync(PermissionService.Table tabel, string caleFisier)
    {
        await using var ctx = AppServices.CreazaContext();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add(NumeFoaie(tabel));

        switch (tabel)
        {
            case PermissionService.Table.Solicitari:
                ScrieAntet(sheet, AntetSolicitari);
                var solicitari = await ctx.Solicitari.AsNoTracking()
                    .OrderByDescending(s => s.Data)
                    .ToListAsync();
                for (var i = 0; i < solicitari.Count; i++)
                    ScrieRandSolicitare(sheet, i + 2, solicitari[i]);
                break;

            case PermissionService.Table.Ingineri:
                ScrieAntet(sheet, AntetIngineri);
                var ingineri = await ctx.Ingineri.AsNoTracking()
                    .OrderBy(i => i.NumeInginer)
                    .ToListAsync();
                for (var i = 0; i < ingineri.Count; i++)
                    ScrieRandInginer(sheet, i + 2, ingineri[i]);
                break;

            case PermissionService.Table.Echipamente:
                ScrieAntet(sheet, AntetEchipamente);
                var echipamente = await ctx.Echipamente.AsNoTracking()
                    .OrderBy(e => e.TipEchipament)
                    .ToListAsync();
                for (var i = 0; i < echipamente.Count; i++)
                    ScrieRandEchipament(sheet, i + 2, echipamente[i]);
                break;

            default:
                throw new ArgumentException("Exportul Excel nu este disponibil pentru acest tabel.");
        }

        sheet.Columns().AdjustToContents();
        workbook.SaveAs(caleFisier);
    }

    public async Task<ExcelImportResult> ImportaAsync(PermissionService.Table tabel, string caleFisier)
    {
        if (!File.Exists(caleFisier))
            throw new FileNotFoundException("Fișierul selectat nu există.", caleFisier);

        using var workbook = new XLWorkbook(caleFisier);
        var sheet = workbook.Worksheet(1);
        var randuri = sheet.RangeUsed()?.RowsUsed().Skip(1).ToList() ?? [];

        if (randuri.Count == 0)
            return new ExcelImportResult { Sarite = 0, Erori = ["Fișierul nu conține date de importat."] };

        await using var ctx = AppServices.CreazaContext();
        var erori = new List<string>();
        var inserate = 0;
        var actualizate = 0;
        var sarite = 0;

        for (var index = 0; index < randuri.Count; index++)
        {
            var rand = randuri[index];
            var nrRand = rand.RowNumber();

            try
            {
                switch (tabel)
                {
                    case PermissionService.Table.Solicitari:
                        var solicitare = CitesteSolicitare(rand);
                        SolicitariService.Valideaza(solicitare);
                        if (await ActualizeazaSauInsereazaSolicitareAsync(ctx, solicitare))
                            actualizate++;
                        else
                            inserate++;
                        break;

                    case PermissionService.Table.Ingineri:
                        var inginer = CitesteInginer(rand);
                        IngineriService.Valideaza(inginer);
                        if (await ActualizeazaSauInsereazaInginerAsync(ctx, inginer))
                            actualizate++;
                        else
                            inserate++;
                        break;

                    case PermissionService.Table.Echipamente:
                        var echipament = CitesteEchipament(rand);
                        EchipamenteService.Valideaza(echipament);
                        if (await ActualizeazaSauInsereazaEchipamentAsync(ctx, echipament))
                            actualizate++;
                        else
                            inserate++;
                        break;

                    default:
                        throw new ArgumentException("Importul Excel nu este disponibil pentru acest tabel.");
                }
            }
            catch (Exception ex)
            {
                sarite++;
                erori.Add($"Rândul {nrRand}: {ex.Message}");
            }
        }

        if (inserate > 0 || actualizate > 0)
            await ctx.SaveChangesAsync();

        return new ExcelImportResult
        {
            Inserate = inserate,
            Actualizate = actualizate,
            Sarite = sarite,
            Erori = erori
        };
    }

    private static readonly string[] AntetSolicitari =
    [
        "ID", "Nume inginer", "Prenume", "Client", "Adresă", "Data",
        "Tip echipament", "Model", "Nr. serie", "Cantitate", "Sumă MDL", "Status"
    ];

    private static readonly string[] AntetIngineri =
    [
        "ID", "Nume", "Prenume", "Echipamente deservite"
    ];

    private static readonly string[] AntetEchipamente =
    [
        "ID", "Tip echipament", "Model", "Nr. serie"
    ];

    private static string NumeFoaie(PermissionService.Table tabel) => tabel switch
    {
        PermissionService.Table.Solicitari => "Solicitari",
        PermissionService.Table.Ingineri => "Ingineri",
        PermissionService.Table.Echipamente => "Echipamente",
        _ => "Date"
    };

    private static void ScrieAntet(IXLWorksheet sheet, string[] antet)
    {
        for (var col = 0; col < antet.Length; col++)
            sheet.Cell(1, col + 1).Value = antet[col];

        sheet.Row(1).Style.Font.Bold = true;
    }

    private static void ScrieRandSolicitare(IXLWorksheet sheet, int rand, Solicitare s)
    {
        sheet.Cell(rand, 1).Value = s.IdSolicitare;
        sheet.Cell(rand, 2).Value = s.NumeInginer;
        sheet.Cell(rand, 3).Value = s.PrenumeInginer;
        sheet.Cell(rand, 4).Value = s.Client;
        sheet.Cell(rand, 5).Value = s.Adresa;
        sheet.Cell(rand, 6).Value = s.Data;
        sheet.Cell(rand, 7).Value = s.TipEchipament;
        sheet.Cell(rand, 8).Value = s.ModelEchipament;
        sheet.Cell(rand, 9).Value = s.NrSerie;
        sheet.Cell(rand, 10).Value = s.CantitateEchipament;
        sheet.Cell(rand, 11).Value = s.SumaAchitare;
        sheet.Cell(rand, 12).Value = s.Status;
    }

    private static void ScrieRandInginer(IXLWorksheet sheet, int rand, Inginer i)
    {
        sheet.Cell(rand, 1).Value = i.IdInginer;
        sheet.Cell(rand, 2).Value = i.NumeInginer;
        sheet.Cell(rand, 3).Value = i.PrenumeInginer;
        sheet.Cell(rand, 4).Value = i.CantitateEchipamenteDeservite;
    }

    private static void ScrieRandEchipament(IXLWorksheet sheet, int rand, Echipament e)
    {
        sheet.Cell(rand, 1).Value = e.IdEchipament;
        sheet.Cell(rand, 2).Value = e.TipEchipament;
        sheet.Cell(rand, 3).Value = e.ModelEchipament;
        sheet.Cell(rand, 4).Value = e.NrSerie;
    }

    private static Solicitare CitesteSolicitare(IXLRangeRow rand)
    {
        if (EsteRandGol(rand, 12))
            throw new InvalidOperationException("Rând gol.");

        var status = CitesteTextOptional(rand, 12);
        return new Solicitare
        {
            IdSolicitare = CitesteIntOptional(rand, 1),
            NumeInginer = CitesteText(rand, 2),
            PrenumeInginer = CitesteText(rand, 3),
            Client = CitesteText(rand, 4),
            Adresa = CitesteText(rand, 5),
            Data = CitesteData(rand, 6),
            TipEchipament = CitesteText(rand, 7),
            ModelEchipament = CitesteText(rand, 8),
            NrSerie = CitesteTextOptional(rand, 9),
            CantitateEchipament = CitesteInt(rand, 10),
            SumaAchitare = CitesteDecimal(rand, 11),
            Status = string.IsNullOrWhiteSpace(status)
                ? StatusSolicitareMapper.ToStorage(StatusSolicitare.Preluat)
                : status
        };
    }

    private static Inginer CitesteInginer(IXLRangeRow rand)
    {
        if (EsteRandGol(rand, 4))
            throw new InvalidOperationException("Rând gol.");

        return new Inginer
        {
            IdInginer = CitesteIntOptional(rand, 1),
            NumeInginer = CitesteText(rand, 2),
            PrenumeInginer = CitesteText(rand, 3),
            CantitateEchipamenteDeservite = CitesteInt(rand, 4)
        };
    }

    private static Echipament CitesteEchipament(IXLRangeRow rand)
    {
        if (EsteRandGol(rand, 4))
            throw new InvalidOperationException("Rând gol.");

        return new Echipament
        {
            IdEchipament = CitesteIntOptional(rand, 1),
            TipEchipament = CitesteText(rand, 2),
            ModelEchipament = CitesteText(rand, 3),
            NrSerie = CitesteText(rand, 4)
        };
    }

    private static async Task<bool> ActualizeazaSauInsereazaSolicitareAsync(AppDbContext ctx, Solicitare solicitare)
    {
        if (solicitare.IdSolicitare > 0)
        {
            var existent = await ctx.Solicitari.FindAsync(solicitare.IdSolicitare);
            if (existent is not null)
            {
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
                return true;
            }
        }

        solicitare.IdSolicitare = 0;
        ctx.Solicitari.Add(solicitare);
        return false;
    }

    private static async Task<bool> ActualizeazaSauInsereazaInginerAsync(AppDbContext ctx, Inginer inginer)
    {
        if (inginer.IdInginer > 0)
        {
            var existent = await ctx.Ingineri.FindAsync(inginer.IdInginer);
            if (existent is not null)
            {
                existent.NumeInginer = inginer.NumeInginer;
                existent.PrenumeInginer = inginer.PrenumeInginer;
                existent.CantitateEchipamenteDeservite = inginer.CantitateEchipamenteDeservite;
                return true;
            }
        }

        inginer.IdInginer = 0;
        ctx.Ingineri.Add(inginer);
        return false;
    }

    private static async Task<bool> ActualizeazaSauInsereazaEchipamentAsync(AppDbContext ctx, Echipament echipament)
    {
        if (echipament.IdEchipament > 0)
        {
            var existent = await ctx.Echipamente.FindAsync(echipament.IdEchipament);
            if (existent is not null)
            {
                if (await ctx.Echipamente.AnyAsync(e => e.NrSerie == echipament.NrSerie && e.IdEchipament != echipament.IdEchipament))
                    throw new ArgumentException($"Numărul de serie '{echipament.NrSerie}' există deja.");

                existent.TipEchipament = echipament.TipEchipament;
                existent.ModelEchipament = echipament.ModelEchipament;
                existent.NrSerie = echipament.NrSerie;
                return true;
            }
        }

        if (await ctx.Echipamente.AnyAsync(e => e.NrSerie == echipament.NrSerie))
            throw new ArgumentException($"Numărul de serie '{echipament.NrSerie}' există deja.");

        echipament.IdEchipament = 0;
        ctx.Echipamente.Add(echipament);
        return false;
    }

    private static bool EsteRandGol(IXLRangeRow rand, int coloane)
    {
        for (var col = 2; col <= coloane; col++)
        {
            if (!string.IsNullOrWhiteSpace(CitesteTextBrut(rand, col)))
                return false;
        }

        return true;
    }

    private static string CitesteText(IXLRangeRow rand, int coloana)
    {
        var valoare = CitesteTextBrut(rand, coloana);
        if (string.IsNullOrWhiteSpace(valoare))
            throw new ArgumentException($"Coloana {coloana} este obligatorie.");

        return valoare.Trim();
    }

    private static string CitesteTextOptional(IXLRangeRow rand, int coloana) =>
        CitesteTextBrut(rand, coloana).Trim();

    private static string CitesteTextBrut(IXLRangeRow rand, int coloana) =>
        rand.Cell(coloana).GetFormattedString().Trim();

    private static int CitesteInt(IXLRangeRow rand, int coloana)
    {
        var celula = rand.Cell(coloana);
        if (celula.TryGetValue(out int valoareInt))
            return valoareInt;

        if (celula.TryGetValue(out double valoareDouble))
            return Convert.ToInt32(valoareDouble);

        var text = CitesteTextBrut(rand, coloana);
        if (int.TryParse(text, out valoareInt))
            return valoareInt;

        throw new ArgumentException($"Coloana {coloana} trebuie să fie un număr întreg.");
    }

    private static int CitesteIntOptional(IXLRangeRow rand, int coloana)
    {
        var text = CitesteTextBrut(rand, coloana);
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return CitesteInt(rand, coloana);
    }

    private static decimal CitesteDecimal(IXLRangeRow rand, int coloana)
    {
        var celula = rand.Cell(coloana);
        if (celula.TryGetValue(out decimal valoareDecimal))
            return valoareDecimal;

        if (celula.TryGetValue(out double valoareDouble))
            return Convert.ToDecimal(valoareDouble);

        var text = CitesteTextBrut(rand, coloana);
        if (decimal.TryParse(text, out valoareDecimal))
            return valoareDecimal;

        throw new ArgumentException($"Coloana {coloana} trebuie să fie un număr.");
    }

    private static DateTime CitesteData(IXLRangeRow rand, int coloana)
    {
        var celula = rand.Cell(coloana);
        if (celula.TryGetValue(out DateTime data))
            return data.Date;

        if (celula.TryGetValue(out double serial))
            return DateTime.FromOADate(serial).Date;

        var text = CitesteTextBrut(rand, coloana);
        if (DateTime.TryParse(text, out data))
            return data.Date;

        throw new ArgumentException($"Coloana {coloana} trebuie să conțină o dată validă.");
    }
}
