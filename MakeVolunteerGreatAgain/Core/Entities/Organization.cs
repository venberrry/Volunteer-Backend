namespace MakeVolunteerGreatAgain.Core.Entities
{
    // Организация
    public class Organization
    {
        public int Id { get; set; }
        public int CommonUserId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoPath { get; set; }
        public string? LegalAddress { get; set; }
        public string? Website { get; set; }
        public string? WorkingHours { get; set; }
        public CommonUser CommonUser { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}