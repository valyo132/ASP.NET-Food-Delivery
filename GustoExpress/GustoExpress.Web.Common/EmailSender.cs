namespace GustoExpress.Web.Common
{
    using System.Net.Mail;
    using System.Net;

    using Microsoft.AspNetCore.Identity.UI.Services;

    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("valentinmarkov132@gmail.com", "xyswbhjqlhukesbg"),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage("valentinmarkov132@gmail.com", email, subject, htmlMessage);
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
