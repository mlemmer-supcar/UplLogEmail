using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("ClientInfoTable")]
public class ClientInfoTable
{
    [Key]
    [Column("ClientID")]
    public int ClientID { get; set; }

    [Column("LastName")]
    public string? LastName { get; set; } = null!;

    [Column("FirstName")]
    public string? FirstName { get; set; } = null!;

    [Column("facility_id")]
    public int? FacilityId { get; set; }

    [Column("test_client")]
    public bool? IsTestClient { get; set; }
}
