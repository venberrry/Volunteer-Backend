namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// Модель организации
public class Organization : User
{
    //[Key]
    //public int IdO { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; }

    public string? PhotoPath { get; set; }

    [Required]
    [MaxLength(500)]
    public string LegalAddress { get; set; }

    public string? Website { get; set; }

    //[Required]
    //[MaxLength(15)]
    //public string PhoneNumber { get; set; }

    //[Required]
    [MaxLength(100)]
    public string WorkingHours { get; set; }

    public ICollection<Event> Events { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    
    //[ForeignKey("User")]
    //public int UserId { get; set; }
   // public User User { get; set; }
}
public enum OrganizationType
{
    RequestingHelp,
    VolunteerGroup
}