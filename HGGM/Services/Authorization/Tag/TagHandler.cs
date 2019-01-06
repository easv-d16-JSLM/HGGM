using System.Threading.Tasks;
using HGGM.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace HGGM.Services.Authorization.Tag
{
    public class TagHandler : AuthorizationHandler<TagRequirement>
    {
        private readonly ILogger _log = Log.ForContext<TagHandler>();
        private readonly UserManager<User> _userManager;

        public TagHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            TagRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);
            _log.Verbose("User {username} needs {requirement} at {context}", user?.UserName, requirement.Tag,
                context.Resource);
            if (user != null)
                if (requirement.Tag.Users.Contains(user) && requirement.Tag != null)

                {
                    _log.Debug("User {username} was granted {requirement}", user?.UserName, requirement.Tag);
                    context.Succeed(requirement);
                }
        }
    }
}