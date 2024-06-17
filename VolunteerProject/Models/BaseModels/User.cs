namespace VolunteerProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
public class User : IdentityUser<int>
{
    public ICollection<Volunteer> Volunteers { get; set; }
    public ICollection<Organization> Organizations { get; set; }
}