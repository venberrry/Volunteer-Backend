namespace MakeVolunteerGreatAgain.Core.Repositories.DTO;

public class UpdateEventDTO
{
    public string? Title { get; set; }
    public string? PhotoPath { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? City { get; set; }
    public string? Description { get; set; }
}