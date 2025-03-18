using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("tbl_state")]
public class StatesTable
{
    [Column("st_id")]
    public int Id { get; set; }

    [Column("st_state")]
    public string State { get; set; } = null!;

    [Column("pcc_active")]
    public bool PccActive { get; set; }
}
