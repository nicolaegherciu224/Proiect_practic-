using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringManagementApp.Models;

[Table("solicitari_audit")]
public class SolicitareAudit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nr_solicitare")]
    public int IdSolicitare { get; set; }

    [Column("status_vechi"), MaxLength(20)]
    public string StatusVechi { get; set; } = string.Empty;

    [Column("status_nou"), MaxLength(20)]
    public string StatusNou { get; set; } = string.Empty;

    [Column("data_schimbare")]
    public DateTime DataSchimbare { get; set; }

    [Column("utilizator"), MaxLength(50)]
    public string Utilizator { get; set; } = string.Empty;

    [Column("observatii"), MaxLength(250)]
    public string? Observatii { get; set; }
}
