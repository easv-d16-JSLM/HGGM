using System.Collections.Generic;
using HGGM.Models.Identity;

namespace HGGM.ViewModels
{
    public class UserWithRolesViewModel
    {
        public string Id { get; set; }
        public List<string> UserRoles { get; set; }
        public List<string> AllRoles { get; set; }
        public string Username { get; set; }
    }
}