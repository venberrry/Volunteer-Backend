using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Repositories.Organization;

public interface IInvitationRepository
{
    Task<IEnumerable<Invitation>> GetAllInvitationsAsync();
    Task<Invitation> GetInvitationByIdAsync(int id);
    Task<Invitation> CreateInvitationAsync(Invitation invitation);
    Task<Invitation> UpdateInvitationAsync(int id, Invitation updatedInvitation);
    Task<Invitation> DeleteInvitationAsync(int id);
}