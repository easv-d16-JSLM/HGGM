using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Configuration
{
    public class MailConfig
    {
        public string Email { get; set; }
        // Security to-be-added
        public string Password { get; set; }
        public string SMTPServer { get; set; }
        public int Port { get; set; }

    }
}
