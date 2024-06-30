namespace MakeVolunteerGreatAgain.Core.Repositories.DTO;

public class RegisterOrganizationDTO
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LegalAddress { get; set; }
}