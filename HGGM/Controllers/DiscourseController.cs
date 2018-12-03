using System.Threading.Tasks;
using Flurl;
using HGGM.Models.Identity;
using HGGM.Services.Discourse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscourseController : ControllerBase
    {
        private readonly DiscourseService _discourseService;
        private readonly UserManager<User> _userManager;

        public DiscourseController(DiscourseService discourseService, UserManager<User> userManager)
        {
            _discourseService = discourseService;
            _userManager = userManager;
        }

        public async Task<IActionResult> SingleSignOn([FromQuery] string sso, [FromQuery] string sig)
        {
            var (nonce, returnUrl) = _discourseService.OpenPayload(sso, sig);
            var user = await _userManager.GetUserAsync(User);

            var (payload, signature) = _discourseService.CreatePayload(nonce, user.Email.Address, user.Id,
                user.UserName, user.Name,
                Url.Action("Avatar", "Files", new {id = user.Id}), user.Biography,
                emailRequireActivation: user.Email.IsConfirmed);

            if (returnUrl == null) returnUrl = Request.Headers["Referer"];

            return Redirect(returnUrl.SetQueryParam("sso", payload).SetQueryParam("sig", signature));
        }
    }
}