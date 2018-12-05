using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class RoleEditAudit : UserActionAudit
    {
        public string Role { get; set; }
        public string RoleId { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public override string Message => $"User '{User}' changed permissions of Role '{Role}'";
    }
}
