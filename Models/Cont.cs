using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EngineeringManagementApp.Enums;

namespace EngineeringManagementApp.Models;

[Table("conturi")]
public class Cont
{
    [Key]
    [Column("ID_account")]
    public int IdAccount { get; set; }

    [Column("login"), MaxLength(50)]
    public string Login { get; set; } = string.Empty;

    [Column("parola"), MaxLength(255)]
    public string Parola { get; set; } = string.Empty;

    [Column("tip_drepturi"), MaxLength(20)]
    public string TipDrepturi { get; set; } = string.Empty;

    [NotMapped]
    public TipDrepturi Rol => TipDrepturiMapper.FromStorage(TipDrepturi);
}
