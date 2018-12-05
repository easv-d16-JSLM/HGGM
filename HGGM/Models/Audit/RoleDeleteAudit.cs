using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class RoleDeleteAudit : UserActionAudit
    {
        public string Role { get; set; }
        public string RoleId { get; set; }
        public override string Message => $"User '{User}' deleted role '{Role}'";
    }
}
