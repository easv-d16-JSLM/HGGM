using HGGM.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.ViewModels
{
    public class UserWithRolesViewModel
    {
        public User User { get; set; }
        public IList<Role> Roles { get; set; }
    }
}
