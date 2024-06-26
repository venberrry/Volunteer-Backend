namespace VolunteerProject.Services.Email;

public interface IEmailService
{

    Task SendEmailAsync(string recipent, string subject, string message);
    Task SendEventSignupEmailAsync(string recipent, string volunteerName, string eventName);

}
