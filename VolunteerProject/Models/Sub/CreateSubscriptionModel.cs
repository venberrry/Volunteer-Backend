using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VolunteerProject.Models;

public class CreateSubscriptionModel
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
}