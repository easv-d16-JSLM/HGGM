using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Serilog;

namespace HGGM.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _log = Log.ForContext<EmailSender>();

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _log.Warning(new NotImplementedException(), "Email to '{user}': '{subject}'\n{body}",email,subject,htmlMessage);

            return Task.CompletedTask;
        }
    }
}
