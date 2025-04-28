public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
    Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachment, string attachmentName);
}