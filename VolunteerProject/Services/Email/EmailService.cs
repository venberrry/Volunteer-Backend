using VolunteerProject.Services.Email;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace VolunteerProject;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    // private readonly SmtpClient _smtpClient;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string recipent, string subject, string message)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
        mailMessage.To.Add(MailboxAddress.Parse(recipent));
        mailMessage.Subject = subject;
        mailMessage.Body = new TextPart(TextFormat.Html) { Text = message };
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
        await smtp.SendAsync(mailMessage);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendEventSignupEmailAsync(string recipent, string volunteerName, string eventName)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EventSignupTemplate.html");
        string templateContent = await File.ReadAllTextAsync(templatePath);

        string emailContent = templateContent
            .Replace("@Model.VolunteerName", volunteerName)
            .Replace("@Model.EventName", eventName);

        await SendEmailAsync(recipent, "Event Signup Confirmation", emailContent);
    }

}
