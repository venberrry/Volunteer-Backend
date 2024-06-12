namespace VolunteerProject.Models.Auth;

using System.ComponentModel.DataAnnotations;

public class RegisterModelOrganization
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    public string Role { get; set; }
}