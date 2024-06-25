namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// Модель организации
public class Organization : User
{
    [Required]
    [MaxLength(200)]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string? ContactEmail { get; set; }
    
    [MaxLength(500)]
    public string? PhotoPath { get; set; }

    [Required]
    [MaxLength(500)]
    public string? LegalAddress { get; set; }
    
    [MaxLength(500)]
    public string? ActualAddress { get; set; }
    
    [MaxLength(500)]
    public string? Website { get; set; }

    //[Required]
    //[MaxLength(15)]
    //public string PhoneNumber { get; set; }

    //[Required]
    [MaxLength(100)]
    public string? WorkingHours { get; set; }

    public ICollection<Event>? Events { get; set; }
    public ICollection<Subscription>? Subscriptions { get; set; }
    public ICollection<Invitation>? Invitations { get; set; }
    
}
public enum OrganizationType
{
    RequestingHelp,
    VolunteerGroup
}