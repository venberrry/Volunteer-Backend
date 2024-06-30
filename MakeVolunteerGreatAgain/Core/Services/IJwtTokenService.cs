using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface IJwtTokenService
{
    string GenerateToken(CommonUser user, IList<string> roles);
}