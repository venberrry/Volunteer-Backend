namespace MakeVolunteerGreatAgain.Core.Repositories.DTO;

public class UpdateVolunteerDTO
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? PhotoPath { get; set; }
    public DateTime BirthDate { get; set; }
    public string? About { get; set; }
    public int? ParticipationCount { get; set; }
}