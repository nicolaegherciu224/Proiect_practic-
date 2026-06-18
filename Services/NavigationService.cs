using System.Windows.Controls;
using EngineeringManagementApp.Views.Pages;

namespace EngineeringManagementApp.Services;

/// <summary>
/// Identifică paginile disponibile în panoul principal.
/// </summary>
public enum PaginaNavigare
{
    Acasa,
    Solicitari,
    Ingineri,
    Echipamente,
    Statistici,
    Conturi
}

/// <summary>
/// Creează paginile și le afișează în Frame-ul principal.
/// Notă: în WPF, Page poate fi afișată doar într-un Frame, nu într-un ContentControl.
/// </summary>
public sealed class NavigationService
{
    private readonly Frame _frame;
    private readonly Dictionary<PaginaNavigare, Page> _pagini = new();

    public NavigationService(Frame frame)
    {
        _frame = frame;
    }

    public PaginaNavigare PaginaCurenta { get; private set; } = PaginaNavigare.Acasa;

    /// <summary>
    /// Navighează către pagina selectată din meniul lateral.
    /// </summary>
    public void Navigheaza(PaginaNavigare pagina)
    {
        if (!_pagini.ContainsKey(pagina))
        {
            _pagini[pagina] = CreazaPagina(pagina);
        }

        PaginaCurenta = pagina;
        _frame.Navigate(_pagini[pagina]);
    }

    // Creează pagina corespunzătoare fiecărei secțiuni
    private static Page CreazaPagina(PaginaNavigare pagina) => pagina switch
    {
        PaginaNavigare.Acasa => new AcasaPage(),
        PaginaNavigare.Solicitari => new SolicitariPage(),
        PaginaNavigare.Ingineri => new IngineriPage(),
        PaginaNavigare.Echipamente => new EchipamentePage(),
        PaginaNavigare.Statistici => new StatisticiPage(),
        PaginaNavigare.Conturi => new ConturiPage(),
        _ => new AcasaPage()
    };
}
