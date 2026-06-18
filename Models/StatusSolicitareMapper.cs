using EngineeringManagementApp.Enums;

namespace EngineeringManagementApp.Models;

public static class StatusSolicitareMapper
{
    public static string ToStorage(StatusSolicitare status) => status switch
    {
        StatusSolicitare.Preluat => "Preluat",
        StatusSolicitare.InLucru => "În Lucru",
        StatusSolicitare.Finalizat => "Finalizat",
        StatusSolicitare.Anulat => "Anulat",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Status necunoscut.")
    };

    public static StatusSolicitare FromStorage(string value) => value switch
    {
        "Preluat" => StatusSolicitare.Preluat,
        "În Lucru" => StatusSolicitare.InLucru,
        "In Lucru" => StatusSolicitare.InLucru,
        "Finalizat" => StatusSolicitare.Finalizat,
        "Anulat" => StatusSolicitare.Anulat,
        _ => StatusSolicitare.Preluat
    };
}
