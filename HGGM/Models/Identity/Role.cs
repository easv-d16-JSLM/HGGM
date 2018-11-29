using System.Collections.Generic;
using AspNetCore.Identity.LiteDB;

namespace HGGM.Models.Identity
{
    public class Role : IdentityRole
    {
        public IList<IPermission> Permissions { get; set; }
    }
}