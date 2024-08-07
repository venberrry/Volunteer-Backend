﻿namespace VolunteerProject.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

// Модель пользователя
public class Volunteer : User
{
    //[Key]
    //public int IdV { get; set; }

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
    public override string Email { get; set; }

    public string? PhotoPath { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    public string? About { get; set; }

    public int? ParticipationCount { get; set; }

    //public string Role { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
    
}