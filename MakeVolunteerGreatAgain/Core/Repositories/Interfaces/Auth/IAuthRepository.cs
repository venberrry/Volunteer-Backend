using MakeVolunteerGreatAgain.Core.Repositories.DTO;

namespace MakeVolunteerGreatAgain.Core.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<AuthResultDTO> RegisterVolunteerAsync(RegisterVolunteerDTO model);
        Task<AuthResultDTO> RegisterOrganizationAsync(RegisterOrganizationDTO model);
        Task<AuthResultDTO> LoginAsync(LoginDTO model);
    }
}