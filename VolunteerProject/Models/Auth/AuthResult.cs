namespace VolunteerProject.Models.Auth;

public class AuthResult
{
    public bool Success { get; set; }
    public List<string>? Errors { get; set; }
    public string? Token { get; set; }  // Свойство для хранения токена
}