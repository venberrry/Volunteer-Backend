using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerProject.Models;

// Модель мероприятия
public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [Required]
    public int OrganizationId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }
    
    [MaxLength(100)]
    public string? PhotoPath { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? City { get; set; }

    [Required] 
    [MaxLength(2000)] 
    public string? Description { get; set; }

    [Required]
    public Organization Organization { get; set; }
    public ICollection<Application> Applications  { get; set; }
}