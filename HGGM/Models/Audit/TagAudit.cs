using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class TagAudit : UserActionAudit
    {
        public string Tag { get; set; }
        public string TagId { get; set; }
        public string Type { get; set; }
        public override string Message => $"User '{User}' {Type} tag '{Tag}'";
    }
}
