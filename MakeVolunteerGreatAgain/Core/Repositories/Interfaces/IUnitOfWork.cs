namespace MakeVolunteerGreatAgain.Core.Repositories;

public interface IUnitOfWork
{
    IVolunteerRepository VolunteerRepository { get; }
    IOrganizationRepository OrganizationRepository { get; }
    Task CommitAsync();
}