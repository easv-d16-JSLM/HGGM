using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HGGM.Services.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger log = Log.ForContext<PermissionHandler>();

        public PermissionHandler(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);
            log.Verbose("User {username} needs {permission} at {context}", user?.UserName, requirement.Permission,
                context.Resource);
            if (_roleManager.Roles.Any(r => user.Roles.Contains(r.Name)
                                            && r.Permissions.Contains(requirement.Permission)))
            {
                log.Debug("User {username} was granted {permission}", user?.UserName, requirement.Permission);
                context.Succeed(requirement);
            }
        }
    }
}