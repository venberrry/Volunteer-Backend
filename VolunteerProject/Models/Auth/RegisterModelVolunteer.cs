namespace VolunteerProject.Models.Auth;

using System.ComponentModel.DataAnnotations;

public class RegisterModelVolunteer
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [Required]
    [MaxLength(30)]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

}