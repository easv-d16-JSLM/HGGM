using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public abstract class UserActionAudit : AuditEntryBase
    {
        public string User { get; set; }
        public string UserId { get; set; }
    }
}
