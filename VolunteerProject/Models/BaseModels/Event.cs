namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Модель мероприятия
public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdE { get; set; }
    
    [Required]
    public int OrganizationId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    
    public string? PhotoPath { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string City { get; set; }

    [Required] 
    [MaxLength(2000)] 
    public string Description { get; set; }

    [Required]
    public Organization Organization { get; set; }
    public ICollection<Application> Applications  { get; set; }
}