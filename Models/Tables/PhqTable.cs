using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("tbl_app_phq9")]
public class PhqTable
{
    [Key]
    [Column("phq_id")]
    public string? PhqId { get; set; }

    [Column("fac_id")]
    public int FacilityId { get; set; }

    [Column("prov_id")]
    public string? ProviderId { get; set; }

    [Column("prov_name")]
    public string? ProviderName { get; set; }

    [Column("phq_dte")]
    public DateTime PhqDate { get; set; }

    [Column("cl_id")]
    public int ClientId { get; set; }

    [Column("cl_name")]
    public string? ClientName { get; set; }

    [Column("phq_enter_dte")]
    public DateTime EnterDate { get; set; }

    [Column("prov_sig")]
    public string? Signature { get; set; }

    [Column("prov_sig_dte")]
    public DateTime SignatureDate { get; set; }

    [Column("interest")]
    public string? Interest { get; set; }

    [Column("down")]
    public string? Down { get; set; }

    [Column("sc_total")]
    public int? Score { get; set; }

    [Column("temp_1")]
    public string? InterviewConducted { get; set; }

    [Column("temp_5")]
    public string? IsDeleted { get; set; }
}
