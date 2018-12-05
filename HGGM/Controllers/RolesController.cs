using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Audit;
using HGGM.Models.Identity;
using HGGM.Services;
using HGGM.Services.Authorization;
using HGGM.Services.Authorization.Simple;
using HGGM.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HGGM.Controllers
{
    [Permission(SimplePermission.SimplePermissionType.GetRoles)]
    public class RolesController : Controller
    {
        private readonly AuditService _auditService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RolesController(RoleManager<Role> roleManager, AuditService auditService, UserManager<User> userManager)
        {
            this._roleManager = roleManager;
            _auditService = auditService;
            _userManager = userManager;
        }

        [Permission(SimplePermission.SimplePermissionType.EditRoles)]
        public ActionResult Create()
        {
            return View();
        }

        [Permission(SimplePermission.SimplePermissionType.EditRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(role);
                _auditService.Add(new RoleCreateAudit()
                {
                    Role = role.Name,
                    RoleId = role.Id,
                    User = _userManager.GetUserName(User),
                    UserId = _userManager.GetUserId(User)
                });

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [Permission(SimplePermission.SimplePermissionType.EditRoles)]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }

        [Permission(SimplePermission.SimplePermissionType.EditRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Role role)
        {
            var roleName = await _roleManager.FindByIdAsync(role.Id);
            await _roleManager.DeleteAsync(role);
            _auditService.Add(new RoleDeleteAudit()
            {
                Role = roleName.Name,
                RoleId = role.Id,
                User = _userManager.GetUserName(User),
                UserId = _userManager.GetUserId(User)
            });
            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> Details(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }

        [Permission(SimplePermission.SimplePermissionType.EditRoles)]
        public async Task<ActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var m = new EditRoleViewModel
            {
                Name = role.Name, SimplePermissions = SimplePermission.GetAllSimplePermissions.ToDictionary(
                    k => k.Permission.ToString(),
                    v => role.Permissions?.OfType<SimplePermission>()?.Contains(v) ?? false)
            };

            return View(m);
        }

        [Permission(SimplePermission.SimplePermissionType.EditRoles)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromForm] EditRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            // Get original role
            var role = await _roleManager.FindByIdAsync(id);
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
            var result = await _roleManager.UpdateAsync(role);

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

            if (!result.Succeeded) return View(model);

            _auditService.Add(new RoleEditAudit
            {
                User = _userManager.GetUserName(User), UserId = _userManager.GetUserId(User), Role = role.Name,
                RoleId = role.Id, Before = JsonConvert.SerializeObject(before),
                After = JsonConvert.SerializeObject(after)
            });

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }
    }
}