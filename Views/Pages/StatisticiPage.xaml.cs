using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using EngineeringManagementApp.Services;

namespace EngineeringManagementApp.Views.Pages;

public partial class StatisticiPage : Page
{
    private readonly StatisticiService _serviciu = new();

    public StatisticiPage()
    {
        InitializeComponent();
    }

    private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e) => _ = IncarcaGraficeAsync();

    private async Task IncarcaGraficeAsync()
    {
        try
        {
            var echipamente = await _serviciu.ObtineDistributieEchipamenteAsync();
            var venituri = await _serviciu.ObtineVenituriLunareAsync();
            var ingineri = await _serviciu.ObtineTopIngineriAsync();

            GraficEchipamente.Series = echipamente
                .Select(e => new PieSeries<int>
                {
                    Name = e.TipEchipament,
                    Values = [e.Cantitate],
                    DataLabelsPaint = new SolidColorPaint(SKColors.DimGray),
                    DataLabelsSize = 12
                })
                .Cast<ISeries>()
                .ToArray();

            GraficVenituri.Series =
            [
                new ColumnSeries<decimal>
                {
                    Name = "Venit MDL",
                    Values = venituri.Select(v => v.Venit).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#1E5AA8"))
                }
            ];
            GraficVenituri.XAxes =
            [
                new Axis
                {
                    Labels = venituri.Select(v => v.Luna).ToArray(),
                    LabelsRotation = 15
                }
            ];

            GraficIngineri.Series =
            [
                new ColumnSeries<int>
                {
                    Name = "Echipamente",
                    Values = ingineri.Select(i => i.EchipamenteReparate).ToArray(),
                    Fill = new SolidColorPaint(SKColor.Parse("#2E7DD1"))
                }
            ];
            GraficIngineri.XAxes =
            [
                new Axis
                {
                    Labels = ingineri.Select(i => i.NumeComplet).ToArray(),
                    LabelsRotation = 20
                }
            ];
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Eroare la încărcarea statisticilor: {ex.Message}", "Eroare",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
