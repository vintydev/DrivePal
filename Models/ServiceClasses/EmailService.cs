using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(emailSettings["Username"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        var bodyBuilder = new BodyBuilder { HtmlBody = message };
        email.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(emailSettings["Host"], int.Parse(emailSettings["Port"]), bool.Parse(emailSettings["UseSsl"]));
        await smtpClient.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
        await smtpClient.SendAsync(email);
        await smtpClient.DisconnectAsync(true);
    }
}