using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface IInvitationService
{
    Task<Invitation> CreateInvitationAsync(Invitation invitation);
    Task<Invitation> UpdateInvitationAsync(int id, Invitation updatedInvitation);
    Task<Invitation?> DeleteInvitationAsync(int id);
    Task<IEnumerable<Invitation?>> GetAllInvitationsAsync();
    Task<Invitation?> GetInvitationByIdAsync(int id);
}