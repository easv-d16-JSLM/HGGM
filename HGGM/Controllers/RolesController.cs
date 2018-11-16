using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> roleManager;


        public RolesController(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(role);

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                if (result.Succeeded) return RedirectToAction(nameof(Index));


            }

            return View();
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Role role)
        {
                await roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.UpdateAsync(role);

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
            }
                 
            return View(role);          
        }

        // GET: Roles
        public ActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }
    }
}