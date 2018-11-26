using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;
using MimeKit;
using MailKit.Net.Smtp;
using HGGM.Models.Configuration;
using Microsoft.Extensions.Configuration;

namespace HGGM.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailConfig _setup;
        private readonly ILogger _log = Log.ForContext<EmailSender>();

        public EmailSender(IConfiguration config)
        {            
            this._setup = config.GetSection("EmailSettings").Get<MailConfig>();
        }
               
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                //From Address    
                string fromAddress = _setup.Email;
                string FromAdressTitle = "HGGM";

                //To Address    
                string ToAddress = email;
                string ToAdressTitle = "User";
                string Subject = subject;
                string BodyContent = message;

                // Setting up server 
                string SmtpServer = _setup.SMTPServer;
                int SmtpPortNumber = _setup.Port;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress
                                        (FromAdressTitle,
                                         fromAddress
                                         ));

                mimeMessage.To.Add(new MailboxAddress
                                         (ToAdressTitle,
                                         ToAddress
                                         ));
                mimeMessage.Subject = Subject; 
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = BodyContent
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    client.Authenticate(
                        _setup.Email,
                        _setup.Password
                        );
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}