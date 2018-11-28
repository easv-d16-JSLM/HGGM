using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models;
using HGGM.Models.Identity;
using HGGM.Services.Authorization.Simple;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class HomeController : Controller
    {
        private readonly LiteRepository _db;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public HomeController(LiteRepository db, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        //TODO: Remove
        public async Task<IActionResult> BecomeAdmin()
        {
            var role = await _roleManager.FindByNameAsync("Admin");
            if (role == null)
                await _roleManager.CreateAsync(new Role
                {
                    Name = "Admin", Permissions = SimplePermission.GetAllSimplePermissions.Cast<IPermission>().ToList()
                });

            var user = await _userManager.GetUserAsync(User);
            user.Roles = new List<string> {"Admin"};
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}