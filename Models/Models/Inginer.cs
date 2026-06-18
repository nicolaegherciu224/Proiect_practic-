using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringManagementApp.Models;

[Table("ingineri")]
public class Inginer
{
    [Key]
    [Column("ID_inginer")]
    public int IdInginer { get; set; }

    [Column("nume"), MaxLength(50)]
    public string NumeInginer { get; set; } = string.Empty;

    [Column("prenume"), MaxLength(50)]
    public string PrenumeInginer { get; set; } = string.Empty;

    [Column("cantitate_echipamente")]
    public int CantitateEchipamenteDeservite { get; set; }

    // Calculat în aplicație: cantitate × 400 MDL
    [NotMapped]
    public decimal SalariuEstimativ => CantitateEchipamenteDeservite * 400m;

    // Pentru afișare în ComboBox
    [NotMapped]
    public string NumeComplet => $"{NumeInginer} {PrenumeInginer}";
}
