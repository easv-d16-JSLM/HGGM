using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using HGGM.Services;
using HGGM.ViewModels;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class UsersController : Controller
    {
        private readonly List<string> _countryList = CultureService.GetCountries();
        private readonly LiteRepository _db;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;


        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager, LiteRepository db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles
                .Select(r => r.Name)
                .ToDictionary(r => r, r => user.Roles.Contains(r));
            return View(new EditUserViewModel
            {
                Username = user.UserName,
                Email = user.Email.Address,
                Roles = roles,
                JoinDate = user.JoinDate,
                Biography = user.Biography,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Name = user.Name,
                TeamspeakUID = user.TeamspeakUID,
                Headline = user.Headline
            });
        }

        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles
                .Select(r => r.Name)
                .ToDictionary(r => r, r => user.Roles.Contains(r));
            return View(new EditUserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email.Address,
                Roles = roles,
                JoinDate = user.JoinDate,
                Biography = user.Biography,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Name = user.Name,
                TeamspeakUID = user.TeamspeakUID,
                Headline = user.Headline,
                CountryList = _countryList
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, EditUserViewModel uvm)
        {
            if (!ModelState.IsValid) return View(uvm);
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = uvm.Username;
            user.Name = uvm.Name;
            user.Biography = uvm.Biography;
            user.Country = uvm.Country;
            user.DateOfBirth = uvm.DateOfBirth;
            user.JoinDate = uvm.JoinDate;
            user.TeamspeakUID = uvm.TeamspeakUID;
            user.Headline = uvm.Headline;
            user.Email = uvm.Email;
            user.Roles = uvm.Roles.Where(r => r.Value).Select(h => h.Key).ToList();
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (uvm.RemoveAvatar)
                {
                    _db.FileStorage.Delete(user.Id);
                }
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Description);

            return View(uvm);
        }

        public ActionResult Index()
        {
            var users = _db.Fetch<User>(collectionName: "users").ToList();
            return View(users);
        }
    }
}