using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HGGM.Models.Audit;
using HGGM.Models.Identity;
using HGGM.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace HGGM.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly AuditService _audit;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            AuditService audit)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _audit = audit;
        }

        public DateTime DateOfBirth { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData] public string StatusMessage { get; set; }

        public string Username { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var dateOfBirth = user.DateOfBirth.Date;
            var teamspeakUid = user.TeamspeakUID;

            Username = userName;
            DateOfBirth = dateOfBirth;

            Input = new InputModel
            {
                Email = email,
                TeamspeakUid = teamspeakUid
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            var before = JsonConvert.SerializeObject(user, Formatting.Indented);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            user.TeamspeakUID = Input.TeamspeakUid;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return NotFound("Unable to load update profile.");
            foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Description);

            _audit.Add(new UserProfileAudit
            {
                User = user.UserName,
                UserId = user.Id,
                Type = "account",
                Before = before,
                After = JsonConvert.SerializeObject(user, Formatting.Indented)
            });

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new {userId, code},
                Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        public class InputModel
        {
            [Required] [EmailAddress] public string Email { get; set; }

            public string TeamspeakUid { get; set; }
        }
    }
}