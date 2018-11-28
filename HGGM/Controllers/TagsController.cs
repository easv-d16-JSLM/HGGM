using System;
using HGGM.Models;
using HGGM.Services.Authorization;
using HGGM.Services.Authorization.Simple;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    [Permission(SimplePermission.SimplePermissionType.GetTags)]
    public class TagsController : Controller
    {
        private readonly LiteRepository db;

        public TagsController(LiteRepository db)
        {
            this.db = db;
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
            db.Delete<Tag>(tagId);

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
                db.Update(tag);

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