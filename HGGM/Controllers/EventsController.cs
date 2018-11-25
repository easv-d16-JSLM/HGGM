using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Events;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class EventsController : Controller
    {

        private readonly LiteRepository db;

        public EventsController(LiteRepository db)
        {
            this.db = db;

        }
        // GET: Event
        public ActionResult Index()
        {
            var events = db.Fetch<Event>();
            return View(events);
        }

        // GET: Event/Details/5
        public ActionResult Details(Guid id)
        {
            var hggmEvent = db.SingleById<Event>(id);
            return View(hggmEvent);
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name")] Event hggEvent)
        {
            if (ModelState.IsValid)
            {
                db.Insert(hggEvent);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Event/Edit/5
        public ActionResult Edit(Guid id)
        {
            var hggmEvent = db.SingleById<Event>(id);
            return View(hggmEvent);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event hggmEvent)
        {
            if (ModelState.IsValid)
            {

                db.Update<Event>(hggmEvent);

                return RedirectToAction(nameof(Index));
            }

            return View(hggmEvent);
        }

        // GET: Event/Delete/5
        public ActionResult Delete(Guid id)
        {
            var hggmEvent = db.SingleById<Event>(id);
            return View(hggmEvent);
        }

        // POST: Event/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(nameof(Event.Id))][FromRoute]Event hggmEvent)
        {
            var eventGuid = hggmEvent.Id;
            db.Delete<Event>(eventGuid);

            return RedirectToAction(nameof(Index));
        }
    }
}