using MakeVolunteerGreatAgain.Core.Entities;

namespace MakeVolunteerGreatAgain.Core.Services;

public interface IInvitationService
{
    Task<Invitation> CreateInvitationAsync(int volunteerCommonUserId, int organizationCommonUserId);
    Task<Invitation> UpdateInvitationAsync(int id, Invitation updatedInvitation);
    Task<Invitation?> DeleteInvitationAsync(int id);
    Task<IEnumerable<Invitation?>> GetAllInvitationsAsync(int organizationId);
    Task<Invitation?> GetInvitationByIdAsync(int id);
    Task<IEnumerable<Invitation>> GetAllInvitationsForVolunteerAsync(int volunteerCommonUserId);
}