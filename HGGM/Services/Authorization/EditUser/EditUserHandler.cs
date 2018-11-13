using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HGGM.Services.Authorization.EditUser
{
    public class EditUserHandler : AuthorizationHandler<EditUserRequirement, User>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public EditUserHandler(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            EditUserRequirement requirement, User resource)
        {
            var editor = await _userManager.GetUserAsync(context.User);
            var editorRoles = await editor.GetRoles(_userManager, _roleManager);
            var editorPermissions = editorRoles.SelectMany(r => r.Permissions).OfType<EditUserPermission>();
            var editeeRoles = await resource.GetRoles(_userManager, _roleManager);

            foreach (var permission in editorPermissions)
                if (permission.PropertyName == requirement.PropertyName)
                    foreach (var allowedEditeeRole in permission.Roles)
                        if (editeeRoles.Contains(allowedEditeeRole))
                            context.Succeed(requirement);
        }
    }
}