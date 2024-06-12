namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
public class EventParticipants
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int EventId { get; set; }
    public Event Event { get; set; }
    
    public int? UserId { get; set; }
    public User User { get; set; }
    
    public int? OrganizationId { get; set; }
    public Organization Organization { get; set; }

    [Required]
    public DateTime JoinDate { get; set; }
}