using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EngineeringManagementApp.Enums;

namespace EngineeringManagementApp.Models;

[Table("solicitari")]
public class Solicitare
{
    [Key]
    [Column("nr_solicitare")]
    public int IdSolicitare { get; set; }

    [Column("nume_inginer"), MaxLength(50)]
    public string NumeInginer { get; set; } = string.Empty;

    [Column("prenume_inginer"), MaxLength(50)]
    public string PrenumeInginer { get; set; } = string.Empty;

    [Column("client"), MaxLength(100)]
    public string Client { get; set; } = string.Empty;

    [Column("adresa"), MaxLength(150)]
    public string Adresa { get; set; } = string.Empty;

    [Column("data")]
    public DateTime Data { get; set; }

    [Column("tip_echipament"), MaxLength(50)]
    public string TipEchipament { get; set; } = string.Empty;

    [Column("model_echipament"), MaxLength(50)]
    public string ModelEchipament { get; set; } = string.Empty;

    [Column("nr_serie"), MaxLength(50)]
    public string NrSerie { get; set; } = string.Empty;

    [Column("cantitate")]
    public int CantitateEchipament { get; set; }

    [Column("suma", TypeName = "decimal(10,2)")]
    public decimal SumaAchitare { get; set; }

    [Column("status"), MaxLength(20)]
    public string Status { get; set; } = StatusSolicitareMapper.ToStorage(StatusSolicitare.Preluat);

    [NotMapped]
    public StatusSolicitare StatusEnum => StatusSolicitareMapper.FromStorage(Status);
}
