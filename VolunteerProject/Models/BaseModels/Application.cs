namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// Модель мероприятия
public class Application
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int VolunteerId { get; set; }

    [Required]
    public int EventId { get; set; }

    [MaxLength(400)]
    public string? CoverLetter { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Volunteer Volunteer { get; set; }
    public Event Event { get; set; }
}