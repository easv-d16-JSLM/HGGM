using HGGM.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> roleManager;



        public RolesController(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }

        // GET: Roles
        public ActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            return View(roleManager.Roles.ElementAt(id));
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name")]Role role)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(role);

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                Role role = await roleManager.FindByIdAsync(id);

                await roleManager.UpdateAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                Role role = await roleManager.FindByIdAsync(id);

                await roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}