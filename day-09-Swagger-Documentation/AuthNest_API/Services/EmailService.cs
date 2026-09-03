using MailKit.Net.Smtp;
using MimeKit;

namespace AuthNest_API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                "AuthNest",
                _configuration["EmailSettings:Username"]
            ));

            email.To.Add(MailboxAddress.Parse(to));

            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _configuration["EmailSettings:Host"],
                int.Parse(_configuration["EmailSettings:Port"]!)
            );

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]
            );

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}