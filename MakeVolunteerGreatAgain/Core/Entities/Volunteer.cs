namespace MakeVolunteerGreatAgain.Core.Entities
{
    // Волонтер
    public class Volunteer
    {
        public int Id { get; set; }
        public int CommonUserId { get; set; }
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhotoPath { get; set; }
        public DateTime BirthDate { get; set; }
        public string? About { get; set; }
        public int? ParticipationCount { get; set; }
        public CommonUser CommonUser { get; set; }
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        
    }
}