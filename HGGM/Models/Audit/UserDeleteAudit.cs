using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class UserDeleteAudit : UserActionAudit
    {
        public string Before { get; set; }
        public string After { get; set; }
        public override string Message => $"User '{User}' deleted him/herself";
    }
}
