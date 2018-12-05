using System.Linq;
using System.Threading.Tasks;
using Flurl;
using HGGM.Models.Identity;
using HGGM.Services.Authorization.Simple;
using HGGM.Services.Discourse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HGGM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscourseController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly DiscourseService _discourseService;
        private readonly ILogger _log = Log.ForContext<DiscourseController>();
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public DiscourseController(DiscourseService discourseService, UserManager<User> userManager,
            RoleManager<Role> roleManager, IAuthorizationService authorizationService)
        {
            _discourseService = discourseService;
            _userManager = userManager;
            _roleManager = roleManager;
            _authorizationService = authorizationService;
        }

        [HttpGet("SingleSignOn")]
        public async Task<RedirectResult> SingleSignOn([FromQuery] string sso, [FromQuery] string sig)
        {
            var (nonce, returnUrl) = _discourseService.OpenPayload(sso, sig);
            var user = await _userManager.GetUserAsync(User);
            _log.Information("Login request from {user}", user.UserName, user.Id, nonce, returnUrl);
            var (payload, signature) = _discourseService.CreatePayload(nonce, user.Email.Address, user.Id,
                user.UserName, user.Name,
                Url.Action("Avatar", "Files", new {id = user.Id}, Request.Scheme, Request.Host.Value), user.Biography,
                user.Roles, _roleManager.Roles.Where(r => !user.Roles.Contains(r.Name)).Select(r => r.Name).ToList(),
                (await _authorizationService.AuthorizeAsync(User, null,
                    SimplePermissionRequirement.For(SimplePermissionType.DiscourseAdmin))).Succeeded,
                (await _authorizationService.AuthorizeAsync(User, null,
                    SimplePermissionRequirement.For(SimplePermissionType.DiscourseModerator))).Succeeded, true, false);
            if (returnUrl == null) returnUrl = Request.Headers["Referer"];
            var url = returnUrl.SetQueryParam("sso", payload).SetQueryParam("sig", signature);
            return Redirect(url);
        }
    }
}