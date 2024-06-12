namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;

// Модель пользователя
public class User
{
    [Key]
    public int Id { get; set; } 
    
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
    public string Password { get; set; }
    
    public ICollection<OrganizationMembership> OrganizationMemberships { get; set; }
    public ICollection<EventParticipants> EventParticipants { get; set; }
}