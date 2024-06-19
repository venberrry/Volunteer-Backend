using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using VolunteerProject.Models;
using VolunteerProject.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<Volunteer> volunteerManager, UserManager<Organization> organizationManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _volunteerManager = volunteerManager;
            _organizationManager = organizationManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<AuthResult> RegisterVolunteerAsync(RegisterModelVolunteer model)
        {
            var volunteer = new Volunteer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = DateTime.SpecifyKind(model.BirthDate, DateTimeKind.Utc),
            };

            var result = await _volunteerManager.CreateAsync(volunteer, model.Password);

            if (!result.Succeeded)
            {
                return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
            }

            await _volunteerManager.AddToRoleAsync(volunteer, "Volunteer");
            var token = GenerateJwtToken(volunteer);
            
            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> RegisterOrganizationAsync(RegisterModelOrganization model)
        {
            var organization = new Organization
            {
                Name = model.Name,
                ContactEmail = model.Email,
                LegalAddress = model.LegalAddress,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email
            };

            var result = await _organizationManager.CreateAsync(organization, model.Password);

            if (!result.Succeeded)
            {
                return new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
            }

            await _organizationManager.AddToRoleAsync(organization, "Organization");
            var token = GenerateJwtToken(organization);
            
            return new AuthResult { Success = true, Token = token };
        }
        
        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new AuthResult { Success = false, Errors = new List<string> { "User does not exist." } };
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return new AuthResult { Success = false, Errors = new List<string> { "Invalid login attempt." } };
            }

            var token = GenerateJwtToken(user);

            return new AuthResult { Success = true, Token = token };
        }
        
        private string GenerateJwtToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Id.ToString())) throw new ArgumentNullException(nameof(user.Id));
            if (string.IsNullOrEmpty(user.UserName)) throw new ArgumentNullException(nameof(user.UserName));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("role", user is Volunteer ? "Volunteer" : "Organization")
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Время действия токена
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
