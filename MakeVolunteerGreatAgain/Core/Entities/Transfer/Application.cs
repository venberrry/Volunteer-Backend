namespace MakeVolunteerGreatAgain.Core.Entities
{
    // Заявка на участие в Event(Мероприятии)
    public class Application
    {
        public int Id { get; set; }
        public int VolunteerId { get; set; }
        public int EventId { get; set; }
        public string? CoverLetter { get; set; }
        public string? Status { get; set; } = ApplicationStatus.UnderСonsideration.ToString();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Volunteer Volunteer { get; set; } = new Volunteer();
        public Event Event { get; set; } = new Event();
    }
}

public enum ApplicationStatus 
{
    UnderСonsideration,
    Accepted,
    Rejected
}