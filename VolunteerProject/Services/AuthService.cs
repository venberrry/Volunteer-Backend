using VolunteerProject.Models.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VolunteerProject.Models;

namespace VolunteerProject.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterModelOrganization model);
    Task<AuthResult> RegisterAsync(RegisterModelVolunteer model);
    Task<AuthResult> LoginAsync(LoginModel model); 
}
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly UserManager<Organization> _organizationManager;
    private readonly SignInManager<User> _signInManager;

    public async Task<AuthResult> RegisterAsync(RegisterModelVolunteer model)
    {
        var user = new User
        {
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            Password = model.Password
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
        }
        return new AuthResult { Success = true };
    }
    
    public async Task<AuthResult> RegisterAsync(RegisterModelOrganization model)
    {
        var organization = new Organization
        {
            Title = model.Title,
            Password = model.Password,
        };

        var result = await _organizationManager.CreateAsync(organization, model.Password);

        if (!result.Succeeded)
        {
            return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
        }
        return new AuthResult { Success = true };
    }

    public async Task<AuthResult> LoginAsync(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (!result.Succeeded)
        {
            return new AuthResult { Success = false, Errors = new List<string> { "Invalid login attempt." } };
        }
        return new AuthResult {Success = true};
    }
}