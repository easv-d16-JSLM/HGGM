using System.Collections.Generic;
using HGGM.Models.Identity;

namespace HGGM.ViewModels
{
    public class UserWithRolesViewModel
    {
        public IList<Role> Roles { get; set; }
        public User User { get; set; }
    }
}