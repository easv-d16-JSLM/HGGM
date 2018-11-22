using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace HGGM.Services
{
    public static class GetRolesExtensions
    {
        public static async Task<List<Role>> GetRoles(this User user, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            var roleNames = await userManager.GetRolesAsync(user);
            var roles = new List<Role>();
            foreach (var roleName in roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                roles.Add(role);
            }

            return roles;
        }
    }
}
