using MakeVolunteerGreatAgain.Core.Repositories.DTO;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface IAuthService
{
    Task<AuthResultDTO> RegisterVolunteerAsync(RegisterVolunteerDTO model);
    Task<AuthResultDTO> RegisterOrganizationAsync(RegisterOrganizationDTO model);
    Task<AuthResultDTO> LoginAsync(LoginDTO model);
}