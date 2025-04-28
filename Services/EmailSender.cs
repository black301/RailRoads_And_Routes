using System.Net;
using System.Net.Mail;

namespace Transport__system_prototype.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var fromAddress = new MailAddress("biocodeocteam4010@gmail.com", "RailRoads & Routes");
            var toAddress = new MailAddress(email);
            const string fromPassword = "hmrj gfwn kdhx cljl";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}