using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
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
        private readonly List<string> _countryList = new List<string>();

        public ProfileModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public Guid Avatar { get; set; }

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
            public List<string> Countries { set; get; }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(_countryList.Contains(R.EnglishName)))
                {
                    _countryList.Add(R.EnglishName);
                }
            }
            _countryList.Sort();

            Input = new InputModel
            {
                Avatar = user.Avatar,
                Biography = user.Biography,
                Country = user.Country,
                Headline = user.Headline,
                Name = user.Name,
                Countries = _countryList
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

            //var country = _countryList.Find(c => c.Contains(user.Country));

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