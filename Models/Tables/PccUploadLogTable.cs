using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace UplLogEmail.Models.Tables;

[Table("tbl_pcc_upl_log")]
public class PccUploadLogTable
{
    [Key]
    [Column("pcc_upl_ID")]
    public int PccUplId { get; set; }

    [Column("pcc_patient_id")]
    public int? PccPatientId { get; set; }

    [Column("orgUuid")]
    public string? OrgUuid { get; set; }

    [Column("facID")]
    public int FacId { get; set; }

    [Column("meta_loc")]
    public string? JsonLocation { get; set; }

    [Column("file_loc")]
    public string? PdfLocation { get; set; }

    [Column("file_type")]
    public string? FileType { get; set; }

    [Column("cr_dte")]
    public DateTime? CreatedDate { get; set; }

    [Column("done_dte")]
    public DateTime? UploadedDate { get; set; }

    [Column("cl_id")]
    public int? SupCareId { get; set; }

    [Column("pcc_resp")]
    public string? PccResponse { get; set; }

    [Column("uniqueID")]
    public string NoteId { get; set; } = null!;

    [Column("ses_dte")]
    public DateTime? SessionDate { get; set; }

    [Column("sub_dte")]
    public DateTime? SubmitDate { get; set; }

    [Column("prov_id")]
    public string? ProviderId { get; set; }
}
