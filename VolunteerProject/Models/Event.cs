namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;

// Модель мероприятия
public class Event
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Description  { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
    
    public ICollection<EventParticipants> Participants { get; set; }
}