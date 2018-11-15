using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Services.Authorization;

namespace HGGM.Models.Identity
{
    public class Role : AspNetCore.Identity.LiteDB.IdentityRole
    {
        public List<Permission> Permissions { get; set; }

    }

   
}
