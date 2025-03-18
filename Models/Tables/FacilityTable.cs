using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("tbl_facility")]
public class FacilityTable
{
    [Key]
    [Column("facility_id")]
    public int SupcareFacId { get; set; }

    [Column("facility_name")]
    public string? FacName { get; set; }

    [Column("PHQ2_send")]
    public int? SendPhq2 { get; set; }

    [Column("email_st_dte")]
    public DateTime? StartDate { get; set; }
}
