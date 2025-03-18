using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("tbl_app_bims_2")]
public class BimsTable
{
    [Key]
    [Column("bims_id")]
    public string? BimsId { get; set; }

    [Column("cl_id")]
    public int SupCareId { get; set; }

    [Column("prov_id")]
    public string? ProviderId { get; set; }

    [Column("prov_name")]
    public string? ProviderName { get; set; }

    [Column("facility_id")]
    public int FacilityId { get; set; }

    [Column("bims_dte")]
    public DateTime BimsDate { get; set; }

    [Column("bims_score")]
    public string? BimsScore { get; set; }

    [Column("bims_sig")]
    public string? Signature { get; set; }

    [Column("bims_sig_dte")]
    public DateTime SignatureDate { get; set; }

    [Column("bims_del")]
    public int IsDeleted { get; set; }

    [Column("enter_dte")]
    public DateTime EnterDate { get; set; }
}
