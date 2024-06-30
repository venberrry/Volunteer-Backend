namespace MakeVolunteerGreatAgain.Core.Entities
{
    // Подписка на Organization(Организацию)
    // Для Volunteer(Волонтера)
    public class Subscription
    {
        public int Id { get; set; }
        public int VolunteerId { get; set; }
        public int OrganizationId { get; set; }
        public string? CoverLetter { get; set; }
        public string Status { get; set; } = "Pending";
        public Volunteer Volunteer { get; set; }  = new Volunteer();
        public Organization Organization { get; set; }  = new Organization();
    }
}