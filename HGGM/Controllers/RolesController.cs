using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using HGGM.Services.Authorization.Simple;
using HGGM.ViewModels;
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
            var m = new EditRoleViewModel
            {
                Name = role.Name, SimplePermissions = SimplePermission.GetAllSimplePermissions.ToDictionary(
                    k => k.Permission.ToString(),
                    v => role.Permissions?.OfType<SimplePermission>()?.Contains(v) ?? false)
            };

            return View(m);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromForm] EditRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var role = await roleManager.FindByIdAsync(id);
            role.Name = model.Name;
            var newSimplePermissions = model.SimplePermissions
                .Where(p => p.Value).Select(p =>
                    new SimplePermission(Enum.Parse<SimplePermission.SimplePermissionType>(p.Key)));
            role.Permissions = (role.Permissions ?? new List<IPermission>())
                .Where(p => p.GetType() != typeof(SimplePermission))
                .Concat(newSimplePermissions)
                .ToList();
            var result = await roleManager.UpdateAsync(role);

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            if (result.Succeeded) return RedirectToAction(nameof(Index));

            return View(model);
        }

        // GET: Roles
        public ActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }
    }
}