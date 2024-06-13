namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// Модель пользователя
public class Volunteer : IdentityUser<int>
{
    [Key]
    public int IdV { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(100)]
    public string? MiddleName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string? PhotoPath { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    public string? About { get; set; }

    public int ParticipationCount { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

}