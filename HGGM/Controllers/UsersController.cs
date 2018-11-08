using System.Linq;
using HGGM.Models.Identity;
using HGGM.ViewModels;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HGGM.Controllers
{
    public class UsersController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly LiteRepository _db;

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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserWithRoles/Edit/5
        public async System.Threading.Tasks.Task<ActionResult> Edit(string id)
        {
            return View();
        }

        // POST: UserWithRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserWithRoles
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var users = _db.Fetch<User>(collectionName: "users").ToList();
            return View(users.Select(u => new UserWithRolesViewModel(){User = u}));
        }
    }
}