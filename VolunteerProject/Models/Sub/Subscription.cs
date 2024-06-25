namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Subscription
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    public int VolunteerId { get; init; }

    [Required]
    public int OrganizationId { get; init; }
    
    public Volunteer Volunteer { get; set; }
    public Organization Organization { get; set; }
}