namespace UplLogEmail.Models.Data;

public class FacilityData
{
    public int Id { get; set; }
    public int PccFacId { get; set; }
    public string? OrgUuid { get; set; }
    public string? FacName { get; set; }
    public List<FacilityContactData>? Contacts { get; set; }
}
