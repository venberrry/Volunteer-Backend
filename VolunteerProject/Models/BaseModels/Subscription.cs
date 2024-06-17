namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Subscription
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdS { get; set; }

    [Required]
    public int VolunteerId { get; set; }

    [Required]
    public int OrganizationId { get; set; }

    [MaxLength(400)]
    public string? CoverLetter { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; }

    public Volunteer Volunteer { get; set; }
    public Organization Organization { get; set; }
}