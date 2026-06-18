using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringManagementApp.Models;

[Table("echipamente")]
public class Echipament
{
    [Key]
    [Column("ID_echipament")]
    public int IdEchipament { get; set; }

    [Column("tip_echipament"), MaxLength(50)]
    public string TipEchipament { get; set; } = string.Empty;

    [Column("model"), MaxLength(50)]
    public string ModelEchipament { get; set; } = string.Empty;

    [Column("nr_serie"), MaxLength(50)]
    public string NrSerie { get; set; } = string.Empty;

    // Pentru afișare în ComboBox
    [NotMapped]
    public string DescriereLunga => $"{TipEchipament} - {ModelEchipament} ({NrSerie})";
}
