using System.ComponentModel.DataAnnotations;
public class CreateEventModel
{
    [Required]
    public int OrganizationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

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
}