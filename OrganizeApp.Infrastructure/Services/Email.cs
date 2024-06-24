using Microsoft.Extensions.Configuration;
using MimeKit;
using OrganizeApp.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace OrganizeApp.Infrastructure.Services
{
    public class Email : IEmail
    {
        private string _hostSmtp;
        private int _port;
        private string _senderEmail;
        private string _senderEmailPassword;
        private string _senderName;

        public Email(IConfiguration configuration)
        {
            _hostSmtp = configuration["Email:HostSmtp"];
            _port = int.Parse(configuration["Email:Port"]);
            _senderEmail = configuration["Email:SenderEmail"];
            _senderEmailPassword = configuration["Email:SenderEmailPassword"];
            _senderName = configuration["Email:SenderName"];
        }

        public async Task Send(string subject, string body, string to, string attachmentPath = null)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_senderName, _senderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = @$"
                <html>
                     <head> 
                     </head>
                     <body>
                        <div style=""font-size: 16px; padding: 10px; font-family: Arial; line-height: 1.4;"">
                            {body}
                        </div>
                     </body>
                </html>
                ";

            if (!string.IsNullOrWhiteSpace(attachmentPath))
                builder.Attachments.Add(attachmentPath);

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_hostSmtp, _port, SecureSocketOptions.Auto);

                await client.AuthenticateAsync(_senderEmail, _senderEmailPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
