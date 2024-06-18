namespace VolunteerProject.Models.Auth;

using System.ComponentModel.DataAnnotations;

public class RegisterModelOrganization
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(500)]
    public string LegalAddress { get; set; }
    
}