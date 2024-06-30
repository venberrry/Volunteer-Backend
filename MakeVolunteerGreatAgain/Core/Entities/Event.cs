namespace MakeVolunteerGreatAgain.Core.Entities
{
    // Мероприятие
    public class Event
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string? Title { get; set; }
        public string? PhotoPath { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
        public Organization Organization { get; set; } = new Organization();
        public ICollection<Application>? Applications { get; set; } = new List<Application>();
    }
}