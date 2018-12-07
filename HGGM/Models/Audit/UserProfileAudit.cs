using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class UserProfileAudit : UserActionAudit
    {
        public string Before { get; set; }
        public string After { get; set; }
        public string Type { get; set; }
        public override string Message => $"User '{User}' edited his/her {Type}";
    }
}
