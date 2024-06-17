﻿namespace VolunteerProject.Models.Auth;

using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}