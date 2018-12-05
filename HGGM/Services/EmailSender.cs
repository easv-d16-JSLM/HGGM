using System.Threading.Tasks;
using HGGM.Models.Configuration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;

namespace HGGM.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptionsMonitor<MailConfig> _mailConfig;
        private readonly ILogger _log = Log.ForContext<EmailSender>();


        public EmailSender(IOptionsMonitor<MailConfig> mailConfig)
        {
            _mailConfig = mailConfig;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            //From Address    
            var fromAddress = _mailConfig.CurrentValue.FromEmail;
            var fromName = _mailConfig.CurrentValue.FromName;

            //To Address    
            var toAddress = email;
            var toAddressTitle = email;
            var bodyContent = message;

            // Setting up server 
            var smtpServer = _mailConfig.CurrentValue.SMTPServer;
            var smtpPortNumber = _mailConfig.CurrentValue.Port;

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
                    _mailConfig.CurrentValue.FromEmail,
                    _mailConfig.CurrentValue.Password
                );
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}