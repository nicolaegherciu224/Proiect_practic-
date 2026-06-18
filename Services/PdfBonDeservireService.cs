using EngineeringManagementApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Generează Bon de Deservire Tehnică în format PDF pentru o solicitare.
/// </summary>
public sealed class PdfBonDeservireService
{
    static PdfBonDeservireService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public void Genereaza(Solicitare solicitare, string caleFisier)
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().Text("BON DE DESERVIRE TEHNICĂ")
                        .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().Text("Engineering Management — Provecta / Syrve")
                        .FontSize(10).FontColor(Colors.Grey.Darken1);
                    col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    col.Item().Text($"Nr. solicitare: #{solicitare.IdSolicitare}").Bold();
                    col.Item().Text($"Data: {solicitare.Data:dd.MM.yyyy}");
                    col.Item().Text($"Status: {solicitare.Status}");
                    col.Item().PaddingTop(16).Text("Date client").Bold().FontSize(13);
                    col.Item().PaddingTop(6).Table(t =>
                    {
                        t.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(140);
                            c.RelativeColumn();
                        });

                        AdaugaRand(t, "Client", solicitare.Client);
                        AdaugaRand(t, "Adresă", solicitare.Adresa);
                    });

                    col.Item().PaddingTop(16).Text("Deservire tehnică").Bold().FontSize(13);
                    col.Item().PaddingTop(6).Table(t =>
                    {
                        t.ColumnsDefinition(c =>
                        {
                            c.ConstantColumn(140);
                            c.RelativeColumn();
                        });

                        AdaugaRand(t, "Inginer", $"{solicitare.NumeInginer} {solicitare.PrenumeInginer}");
                        AdaugaRand(t, "Tip echipament", solicitare.TipEchipament);
                        AdaugaRand(t, "Model", solicitare.ModelEchipament);
                        if (!string.IsNullOrWhiteSpace(solicitare.NrSerie))
                            AdaugaRand(t, "Nr. serie", solicitare.NrSerie);
                        AdaugaRand(t, "Cantitate", solicitare.CantitateEchipament.ToString());
                        AdaugaRand(t, "Sumă spre achitare", $"{solicitare.SumaAchitare:N2} MDL");
                    });

                    col.Item().PaddingTop(24).Text(
                        "Prin prezentul document se confirmă efectuarea deservirii tehnice a echipamentului menționat mai sus, " +
                        "conform solicitării clientului.")
                        .FontSize(10).Italic().FontColor(Colors.Grey.Darken1);

                    col.Item().PaddingTop(40).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().LineHorizontal(1);
                            c.Item().PaddingTop(4).Text("Semnătura inginerului").FontSize(10);
                        });
                        row.ConstantItem(40);
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().LineHorizontal(1);
                            c.Item().PaddingTop(4).Text("Semnătura clientului").FontSize(10);
                        });
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Generat la ").FontSize(9).FontColor(Colors.Grey.Medium);
                    text.Span($"{DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(9).FontColor(Colors.Grey.Medium);
                });
            });
        }).GeneratePdf(caleFisier);
    }

    private static void AdaugaRand(TableDescriptor t, string eticheta, string valoare)
    {
        t.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
            .PaddingVertical(6).Text(eticheta).SemiBold();
        t.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3)
            .PaddingVertical(6).Text(valoare);
    }
}
