using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("tbl_pcc_fac")]
public class PccFacilityTable
{
    [Key]
    [Column("pcc_fac_id")]
    public int Id { get; set; }

    [Column("fac_id")]
    public int SupcareFacId { get; set; }

    [Column("pcc_orgUid")]
    public string? PccOrgUuid { get; set; }

    [Column("Pcc_facID")]
    public int PccFacId { get; set; }

    [Column("evalDocID")]
    public int EvalDocId { get; set; }

    [Column("bimsDocID")]
    public int BimsDocId { get; set; }

    [Column("phqDocID")]
    public int PhqDocId { get; set; }

    [Column("progDocID")]
    public int FollowupDocId { get; set; }

    [Column("aimsDocID")]
    public int AimsDocId { get; set; }

    [Column("psychDocID")]
    public int PsychDocId { get; set; }

    [Column("st_dte")]
    public DateTime? StartDate { get; set; }

    [Column("pcc_active")]
    public int IsPccActive { get; set; }
}
