using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models;
using HGGM.Models.Identity;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HGGM.Controllers
{
    public class TagsController : Controller
    {
        private readonly LiteRepository db;

        public TagsController(LiteRepository db)
        {
            this.db = db;
            
        }

        // GET: Tags
        public ActionResult Index()
        {
            var tags = db.Fetch<Tag>();
            return View(tags);
        }

        // GET: Tags/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind("TagName")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Insert(tag);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(ObjectId id)
        {
            return View();
        }

        // POST: Tags/Edit/5
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

        // GET: Tags/Delete/5
        public ActionResult Delete(Guid id)
        {
            var tag = db.SingleById<Tag>(id);
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(nameof(Tag.Id))][FromRoute]Tag tag)
        {
            var tagId = tag.Id;
            db.Delete<Tag>(tagId);
            
            return RedirectToAction(nameof(Index));
        }
    }
}