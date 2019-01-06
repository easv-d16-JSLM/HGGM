using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Events;
using HGGM.Models.Identity;
using HGGM.Services;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HGGM.Controllers
{
    public class EventsController : Controller
    {
        private readonly LiteRepository _db;
        private readonly EventManager _eventManager;
        private readonly UserManager<User> _userManager;

        public EventsController(LiteRepository db, UserManager<User> userManager, EventManager eventManager)
        {
            _db = db;
            _userManager = userManager;
            _eventManager = eventManager;
        }

        public ActionResult AdminView()
        {
            var events = _db.Fetch<Event>();
            return View(events);
        }

        public ActionResult AuditLogIndex()
        {
            return View();
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Event hggEvent)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            hggEvent.Author = user;

            if (ModelState.IsValid)
            {
                _db.Insert(hggEvent);

                return Json("OK");

                //return RedirectToAction(nameof(PublishedIndex));
            }

            return BadRequest();
        }

        public ActionResult CreatedEvents()
        {
            var events = _db.Fetch<Event>();
            var sortedEvents = new List<Event>();
            foreach (var eEvent in events)
                if (eEvent.Publisher == null)
                    sortedEvents.Add(eEvent);
            return View(sortedEvents);
        }

        // GET: Event/Delete/5
        public ActionResult Delete(Guid id)
        {
            var hggmEvent = _db.SingleById<Event>(id);
            return View(hggmEvent);
        }

        // POST: Event/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(nameof(Event.Id))] [FromRoute] Event hggmEvent)
        {
            var eventGuid = hggmEvent.Id;
            _db.Delete<Event>(eventGuid);

            return RedirectToAction(nameof(PublishedIndex));
        }

        // GET: Event/Details/5
        public ActionResult Details(Guid id)
        {
            var hggmEvent = _db.SingleById<Event>(id);
            return View(hggmEvent);
        }

        // GET: Event/Edit/5
        public ActionResult Edit(Guid id)
        {
            var hggmEvent = _db.SingleById<Event>(id);
            return View(hggmEvent);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event hggmEvent)
        {
            if (ModelState.IsValid)
            {
                _db.Update(hggmEvent);

                return RedirectToAction(nameof(PublishedIndex));
            }

            return View(hggmEvent);
        }

        public ActionResult EventsSignedUpFor(User user)
        {
            var events = _db.Fetch<Event>();
            var signedUpEvents = events
                .Where(e => e.Roster
                    .SelectMany(r => r.SignUps).Any(t => t.User.Id == user.Id))
                .ToList();
            return View(signedUpEvents);
        }

        public ActionResult PublishedIndex()
        {
            var events = _db.Fetch<Event>();
            var sortedEvents = new List<Event>();
            foreach (var eEvent in events)
                if (eEvent.Publisher != null)
                    sortedEvents.Add(eEvent);
            return View(sortedEvents);
        }
    }
}