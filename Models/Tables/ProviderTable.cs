using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UplLogEmail.Models.Tables;

[Table("ProviderTable")]
public class ProviderTable
{
    [Key]
    [Column("ProviderID")]
    public string? ProviderId { get; set; }

    [Column("ProviderName")]
    public string? ProviderName { get; set; }
}
