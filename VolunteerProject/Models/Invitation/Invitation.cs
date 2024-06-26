using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolunteerProject.Models;

public class Invitation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required] 
    public int OrganizationId { get; set; }

    [Required] 
    public int VolunteerId { get; set; }

    public Volunteer Volunteer { get; set; }
    public Organization Organization { get; set; }
}