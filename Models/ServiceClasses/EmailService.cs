using MimeKit;
using MailKit.Net.Smtp;

namespace DrivePal.Models.ServiceClasses
{
    /// <summary>
    /// Service class for sending emails using the MailKit package.
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="toEmail">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            //Creates a new MimeMessage instance
            var emailMessage = new MimeMessage();

            //Sets the sender and recipient of the email
            emailMessage.From.Add(new MailboxAddress("*****Header for your email account*******", "******your email*****"));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            // Sets the subject and body of the email
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            //Creates a new SmtpClient instance and sends the email
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.office365.com", 587, false);
                await client.AuthenticateAsync("******your email*******", "*****your password****");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
