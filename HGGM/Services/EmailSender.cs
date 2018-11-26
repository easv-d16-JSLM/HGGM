using System.Threading.Tasks;
using HGGM.Models.Configuration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Serilog;

namespace HGGM.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _log = Log.ForContext<EmailSender>();
        private readonly MailConfig _setup;

        public EmailSender(IConfiguration config)
        {
            _setup = config.GetSection("EmailSettings").Get<MailConfig>();
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            //From Address    
            var fromAddress = _setup.FromEmail;
            var fromName = _setup.FromName;

            //To Address    
            var toAddress = email;
            var toAddressTitle = email;
            var bodyContent = message;

            // Setting up server 
            var smtpServer = _setup.SMTPServer;
            var smtpPortNumber = _setup.Port;

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress
            (fromName,
                fromAddress
            ));

            mimeMessage.To.Add(new MailboxAddress
            (toAddressTitle,
                toAddress
            ));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = bodyContent
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, smtpPortNumber, false);
                client.Authenticate(
                    _setup.FromEmail,
                    _setup.Password
                );
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}