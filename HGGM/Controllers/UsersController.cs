using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using HGGM.ViewModels;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class UsersController : Controller
    {
        private readonly LiteRepository _db;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager, LiteRepository db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        // GET: UserWithRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserWithRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserWithRoles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserWithRoles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserWithRoles/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        // GET: UserWithRoles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles.ToList();
            return View(new UserWithRolesViewModel {Username = user.UserName});
        }

        // POST: UserWithRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserWithRolesViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                user.UserName = uvm.Username;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) { 

                return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var VARIABLE in result.Errors)
                    {
                        ModelState.AddModelError(VARIABLE.Code, VARIABLE.Description);
                    }
                }
            } 

            return View(uvm);
        }

        // GET: UserWithRoles
        public async Task<ActionResult> Index()
        {
            var users = _db.Fetch<User>(collectionName: "users").ToList();
            var roles = _roleManager.Roles.ToList();
            return View(users.Select(u => new UserWithRolesViewModel {Username = u.UserName, Id = u.Id}));
        }
    }
}