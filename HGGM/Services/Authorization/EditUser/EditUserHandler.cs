using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HGGM.Services.Authorization.EditUser
{
    public class EditUserHandler : AuthorizationHandler<EditUserRequirement, User>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger log = Log.ForContext<EditUserHandler>();

        public EditUserHandler(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditUserRequirement requirement, User resource)
        {
            var editor = await _userManager.GetUserAsync(context.User);
            var editee = resource;
            var property = requirement.PropertyName;


        }
    }
}
