namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;

// Модель запроса на вступление в организацию
public class JoinRequest
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }
    
    [Required]
    public int OranizationId { get; set; }
    public Organization Organization { get; set; }
    
    public bool IsApproved { get; set; }
}