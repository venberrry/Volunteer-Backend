namespace MakeVolunteerGreatAgain.Core.Entities
{
    // Приглашение от Organization(Организации) к Волонтеру(Volunteer).
    // Для получения уведомлений о мероприятиях.
    public class Invitation
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int VolunteerId { get; set; }
        public string Status { get; set; } = InvitationStatus.Pending.ToString();
        public Volunteer Volunteer { get; set; } = new Volunteer();
        public Organization Organization { get; set; } = new Organization();

    }
    public enum InvitationStatus 
    {
        Pending,
        Accepted,
        Rejected
    }
}