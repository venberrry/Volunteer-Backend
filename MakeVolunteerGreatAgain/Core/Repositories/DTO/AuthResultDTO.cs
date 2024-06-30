namespace MakeVolunteerGreatAgain.Core.Repositories.DTO;

public class AuthResultDTO
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}