using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class RoleAudit : UserActionAudit
    {
        public string Role { get; set; }
        public string RoleId { get; set; }
        public string Type { get; set; }
        public override string Message => $"User '{User}' {Type} role '{Role}'";
    }
}
