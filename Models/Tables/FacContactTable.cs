using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("tbl_fac_contact")]
public class FacContactTable
{
    [Key]
    [Column("fac_contact_id")]
    public int FacContactId { get; set; }

    [Column("facility_id")]
    public int FacilityId { get; set; }

    [Column("fac_con_email")]
    public string UplLogEmail { get; set; } = null!;

    [Column("fac_notes")]
    public bool? SendNotes { get; set; }

    [Column("fac_batch")]
    public bool? SendBatch { get; set; }

    [Column("fac_auto_upload")]
    public bool? AutoUpload { get; set; }
}
