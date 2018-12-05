using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Audit
{
    public class UserRegisterAudit : UserActionAudit
    {
        public override string Message => $"User '{User}' registered an account";
    }
}
