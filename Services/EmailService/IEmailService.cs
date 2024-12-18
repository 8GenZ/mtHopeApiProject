namespace mtHopeApiProject.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
