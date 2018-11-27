using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using HGGM.Services;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HGGM.Areas.Identity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public List<string> CountryList => CultureService.GetCountries();
        private readonly LiteRepository _db;

        public ProfileModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            LiteRepository db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [StringLength(500, ErrorMessage = "The {0} must be {1} characters long.", MinimumLength = 0)]
            [Display(Name = "Biography")]
            public string Biography { get; set; }

            [StringLength(50, ErrorMessage = "The {0} must be {1} characters long.", MinimumLength = 0)]
            [Display(Name = "Headline")]
            public string Headline { get; set; }

            [RegularExpression("[a-zA-Z]+", ErrorMessage = "The {0} must be letters")]
            [Display(Name = "Real Name")]
            public string Name { get; set; }

            public string Country { get; set; }

            public IFormFile AvatarUpload { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel
            {
                Biography = user.Biography,
                Country = user.Country,
                Headline = user.Headline,
                Name = user.Name
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            user.Name = Input.Name;
            user.Country = Input.Country;
            user.Biography = Input.Biography;
            user.Headline = Input.Headline;
            user.Country = Input.Country;

            if (Input.AvatarUpload != null && Input.AvatarUpload.Length > 0)
            {
                _db.FileStorage.Upload(user.Id, Input.AvatarUpload.FileName, Input.AvatarUpload.OpenReadStream());
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return NotFound($"Unable to load update profile.");
            foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Description);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}