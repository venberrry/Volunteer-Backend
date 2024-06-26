using VolunteerProject.Models.Auth;

namespace VolunteerProject.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterVolunteerAsync(RegisterModelVolunteer model);
    Task<AuthResult> RegisterOrganizationAsync(RegisterModelOrganization model);
    Task<AuthResult> LoginAsync(LoginModel model);
}
