using System;
using System.Linq;
using HGGM.Models;
using HGGM.Models.Audit;
using HGGM.Models.Identity;
using HGGM.Services;
using HGGM.Services.Authorization;
using HGGM.Services.Authorization.Simple;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HGGM.Controllers
{
    [Permission(SimplePermission.SimplePermissionType.GetTags)]
    public class TagsController : Controller
    {
        private readonly LiteRepository db;
        private readonly AuditService _audit;
        private readonly UserManager<User> _userManager;

        public TagsController(LiteRepository db, AuditService audit, UserManager<User> userManager)
        {
            this.db = db;
            _audit = audit;
            _userManager = userManager;
        }

        [Permission(SimplePermission.SimplePermissionType.EditTags)]
        public ActionResult Create()
        {
            return View();
        }

        [Permission(SimplePermission.SimplePermissionType.EditTags)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("TagName")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Insert(tag);
                _audit.Add(new TagAudit()
                {
                    Tag = tag.TagName,
                    TagId = tag.Id.ToString(),
                    Type = "created",
                    User = _userManager.GetUserName(User),
                    UserId = _userManager.GetUserId(User)
                });
                return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [Permission(SimplePermission.SimplePermissionType.EditTags)]
        public ActionResult Delete(Guid id)
        {
            var tag = db.SingleById<Tag>(id);
            return View(tag);
        }

        [Permission(SimplePermission.SimplePermissionType.EditTags)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(nameof(Tag.Id))] [FromRoute] Tag tag)
        {
            var tagId = tag.Id;
            var tagObj = db.SingleById<Tag>(tag.Id);
            db.Delete<Tag>(tagId);

            _audit.Add(new TagAudit()
            {
                Tag = tagObj.TagName,
                TagId = tag.Id.ToString(),
                Type = "deleted",
                User = _userManager.GetUserName(User),
                UserId = _userManager.GetUserId(User)
            });

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(Guid id)
        {
            var tag = db.SingleById<Tag>(id);
            return View(tag);
        }

        [Permission(SimplePermission.SimplePermissionType.EditTags)]
        public ActionResult Edit(Guid id)
        {
            var tag = db.SingleById<Tag>(id);
            return View(tag);
        }

        [Permission(SimplePermission.SimplePermissionType.EditTags)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var tagOld = db.SingleById<Tag>(tag.Id);
                var before = JsonConvert.SerializeObject(tagOld, Formatting.Indented);
                db.Update(tag);
                _audit.Add(new TagEditAudit()
                {
                    Tag = tagOld.TagName,
                    TagId = tag.Id.ToString(),
                    Before = before,
                    After = JsonConvert.SerializeObject(tag, Formatting.Indented),
                    Type = "changes",
                    User = _userManager.GetUserName(User),
                    UserId = _userManager.GetUserId(User)
                });
                return RedirectToAction(nameof(Index));
            }

            return View(tag);
        }

        public ActionResult Index()
        {
            var tags = db.Fetch<Tag>();
            return View(tags);
        }
    }
}