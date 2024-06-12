namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;

// Модель организации
public class Organization
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string Title { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public OrganizationType Type { get; set; }
    
    public ICollection<Event> Events { get; set; }
    public ICollection<JoinRequest> JoinRequests { get; set; }
    public ICollection<OrganizationMembership> Memberships { get; set; }
}
public enum OrganizationType
{
    RequestingHelp,
    VolunteerGroup
}