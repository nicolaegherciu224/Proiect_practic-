using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EngineeringManagementApp.Helpers;

/// <summary>
/// Evidențiere vizuală a câmpurilor invalide în formulare.
/// </summary>
public static class FormValidationHelper
{
    public static void SeteazaValid(TextBox box, bool valid)
    {
        box.BorderBrush = valid
            ? (Brush)Application.Current.FindResource("BorderBrush")
            : (Brush)Application.Current.FindResource("ErrorBrush");
        box.BorderThickness = new Thickness(valid ? 1 : 2);
    }

    public static void Reseteaza(TextBox box) => SeteazaValid(box, true);

    public static void ReseteazaToate(params TextBox[] boxes)
    {
        foreach (var box in boxes)
            Reseteaza(box);
    }
}
