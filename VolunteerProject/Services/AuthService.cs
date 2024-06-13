using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using VolunteerProject.Models;
using VolunteerProject.Models.Auth;

namespace VolunteerProject.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterVolunteerAsync(RegisterModelVolunteer model);
        Task<AuthResult> RegisterOrganizationAsync(RegisterModelOrganization model);
        Task<AuthResult> LoginAsync(LoginModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<Volunteer> _volunteerManager;
        private readonly UserManager<Organization> _organizationManager;
        private readonly SignInManager<Volunteer> _signInManager;

        public AuthService(UserManager<Volunteer> volunteerManager, UserManager<Organization> organizationManager, SignInManager<Volunteer> signInManager)
        {
            _volunteerManager = volunteerManager;
            _organizationManager = organizationManager;
            _signInManager = signInManager;
        }

        public async Task<AuthResult> RegisterVolunteerAsync(RegisterModelVolunteer model)
        {
            var user = new Volunteer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _volunteerManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
            }
            return new AuthResult { Success = true };
        }

        public async Task<AuthResult> RegisterOrganizationAsync(RegisterModelOrganization model)
        {
            var organization = new Organization
            {
                Name = model.Name,
                ContactEmail = model.Email,
                PhoneNumber = model.PhoneNumber,
                LegalAddress = model.LegalAddress
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
            return new AuthResult { Success = true };
        }
    }
}
