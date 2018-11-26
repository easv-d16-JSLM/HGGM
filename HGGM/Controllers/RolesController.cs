using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Audit;
using HGGM.Models.Identity;
using HGGM.Services;
using HGGM.Services.Authorization.Simple;
using HGGM.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HGGM.Controllers
{
    public class RolesController : Controller
    {
        private readonly AuditService _auditService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> roleManager;

        public RolesController(RoleManager<Role> roleManager, AuditService auditService, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            _auditService = auditService;
            _userManager = userManager;
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
            // Get original role
            var role = await roleManager.FindByIdAsync(id);
            // Change properties
            role.Name = model.Name;
            var before = role.Permissions ?? new List<IPermission>();
            var after = model.SimplePermissions
                .Where(p => p.Value).Select(p =>
                    new SimplePermission(Enum.Parse<SimplePermission.SimplePermissionType>(p.Key)));
            role.Permissions = before
                .Where(p => p.GetType() != typeof(SimplePermission))
                .Concat(after)
                .ToList();
            //Save
            var result = await roleManager.UpdateAsync(role);

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            if (!result.Succeeded) return View(model);

            _auditService.Add(new RoleEditAudit
            {
                User = _userManager.GetUserName(User), UserId = _userManager.GetUserName(User), Role = role.Name,
                RoleId = role.Id, Before = JsonConvert.SerializeObject(before),
                After = JsonConvert.SerializeObject(after)
            });

            return RedirectToAction(nameof(Index));
        }

        // GET: Roles
        public ActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }
    }
}