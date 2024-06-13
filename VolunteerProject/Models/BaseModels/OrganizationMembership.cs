namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
public class OrganizationMembership
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int OranizationId { get; set; }
    public Organization Organization { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }
    
    [Required]
    public DateTime JoinDate { get; set; }
}