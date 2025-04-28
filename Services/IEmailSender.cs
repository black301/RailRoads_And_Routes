using System.Threading.Tasks;

namespace Transport__system_prototype.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}