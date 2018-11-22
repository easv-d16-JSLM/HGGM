using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Services.Authorization;
using HGGM.Services.Authorization.Simple;

namespace HGGM.Models.Identity
{
    public class Role : AspNetCore.Identity.LiteDB.IdentityRole
    {
        public IList<IPermission> Permissions { get; set; }
    }

   
}
