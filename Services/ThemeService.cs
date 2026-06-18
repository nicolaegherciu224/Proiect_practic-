using System.Windows;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Schimbă tema aplicației între mod deschis și mod întunecat.
/// </summary>
public static class ThemeService
{
    // true = temă întunecată, false = temă deschisă
    public static bool EsteTemaIntunecata { get; private set; }

    // Eveniment declanșat după fiecare schimbare de temă (pentru actualizarea etichetei butonului)
    public static event Action? TemaSchimbata;

    /// <summary>
    /// Aplică tema dorită înlocuind dicționarul de culori din resursele aplicației.
    /// </summary>
    public static void AplicaTema(bool temaIntunecata)
    {
        var aplicatie = Application.Current;
        var resurse = aplicatie.Resources.MergedDictionaries;

        // Găsim și înlocuim dicționarul de temă existent
        ResourceDictionary? temaVeche = null;
        foreach (var dict in resurse)
        {
            var sursa = dict.Source?.OriginalString ?? string.Empty;
            if (sursa.Contains("ThemeLight") || sursa.Contains("ThemeDark"))
            {
                temaVeche = dict;
                break;
            }
        }

        if (temaVeche is not null)
        {
            resurse.Remove(temaVeche);
        }

        var caleTema = temaIntunecata
            ? "Resources/ThemeDark.xaml"
            : "Resources/ThemeLight.xaml";

        resurse.Add(new ResourceDictionary
        {
            Source = new Uri(caleTema, UriKind.Relative)
        });

        EsteTemaIntunecata = temaIntunecata;
        TemaSchimbata?.Invoke();
    }

    /// <summary>
    /// Comută între temă deschisă și temă întunecată.
    /// </summary>
    public static void ComutaTema()
    {
        AplicaTema(!EsteTemaIntunecata);
    }

    /// <summary>
    /// Textul afișat pe butonul de comutare temă.
    /// </summary>
    public static string EtichetaButonTema =>
        EsteTemaIntunecata ? "☀ Mod deschis" : "🌙 Mod întunecat";
}
