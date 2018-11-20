using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class LoginAudit : AuditEntryBase
    {
        public string Identity { get; set; }
        public string Ip { get; set; }
        public string Provider { get; set; }
        public override string Message => $"User logged in using '{Identity}' with '{Provider}' from '{Ip}'";
    }
}
