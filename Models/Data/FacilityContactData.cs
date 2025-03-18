namespace UplLogEmail.Models.Data;

public class FacilityContactData
{
    public int Id { get; set; }
    public int FacilityId { get; set; }
    public string UplLogEmail { get; set; } = null!;
    public bool SendNotes { get; set; }
    public bool SendBatch { get; set; }
}
