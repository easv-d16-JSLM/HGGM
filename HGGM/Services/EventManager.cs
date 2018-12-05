using System;
using System.Collections.Generic;
using HGGM.Models;
using HGGM.Models.Events;
using HGGM.Models.Identity;
using HGGM.Services.Authorization.Tag;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace HGGM.Services
{
    public class EventManager
    {
        private readonly LiteRepository _db;
        private readonly IStringLocalizer<EventManager> _localizer;
        private readonly INotificationService _nService;
        private readonly IAuthorizationService _authorization;

        public EventManager(LiteRepository db, INotificationService nService, IStringLocalizer<EventManager> localizer, IAuthorizationService authorization)
        {
            _db = db;
            _nService = nService;
            _localizer = localizer;
            _authorization = authorization;
        }

        public void AddEvent(Event eEvent)
        {
            eEvent.Created = DateTimeOffset.Now;
            _db.Insert(eEvent);

            

            _nService.NotifyUsers(new Notification
            {
                Message = _localizer["createEventMessage", eEvent.Author, eEvent.TakesPlace],
                Subject = _localizer["createEventSubject", eEvent.Name]
            }, new List<User>(_db.Fetch<User>()));
            //todo Change to specific users with required roles
        }

        public void AddToSlot(Event eEvent, User user, string note, Slot chosenSlot)
        {
            var slotSignUp = new SlotSignUp
            {
                Created = DateTimeOffset.Now,
                Note = note,
                User = user
            };

            var spot = eEvent.Roster.IndexOf(chosenSlot);
            chosenSlot.SignUps.Add(slotSignUp);
            //eEvent.Roster.RemoveAt(spot);
            eEvent.Roster.Insert(spot, chosenSlot);

            var list = new List<User>
            {
                eEvent.Author,
                user
            };

            _nService.NotifyUsers(new Notification
            {
                Message = "Added to slot",
                Subject = "Event" + eEvent.Name + " Slot added"
            }, list);
        }

        private bool CanUserJoinSlot(HttpContext context)
        {
            var authorizationService = context.RequestServices.GetRequiredService<IAuthorizationService>();
            var result = authorizationService
                .AuthorizeAsync(context.User, null, new TagRequirement()).GetAwaiter()
                .GetResult();

            return result.Succeeded;
        }

        public void DeclineEvent(Guid id, string declineMessage)
        {
            var eEvent = _db.SingleById<Event>(id);
            var list = new List<User>
            {
                eEvent.Author
            };

            _nService.NotifyUsers(new Notification
            {
                Message = declineMessage,
                Subject = "Event" + eEvent.Name + " declined"
            }, list);
        }

        public void DeleteEvent(Guid id)
        {
            var eEvent = _db.SingleById<Event>(id);
            _db.Delete<Event>(id);
            _nService.NotifyUsers(new Notification
            {
                Message = "Event with name: " + eEvent.Name + " deleted!",
                Subject = "Event" + eEvent.Name + " deleted"
            }, GetUsersFromEvent(eEvent));
        }

        public void EditEvent(Event eEvent)
        {
            _db.Update(eEvent);
            _nService.NotifyUsers(new Notification
            {
                Message = "Event with name: " + eEvent.Name + " updated!",
                Subject = "Event" + eEvent.Name + " updated"
            }, GetUsersFromEvent(eEvent));
        }

        public List<User> GetUsersFromEvent(Event eEvent)
        {
            var rosterList = eEvent.Roster;
            var listOfUsersSignedUp = new List<User>();

            foreach (var slot in rosterList)
            {
                var slotSignUps = slot.SignUps;

                foreach (var user in slotSignUps) listOfUsersSignedUp.Add(user.User);
            }

            return listOfUsersSignedUp;
        }

        public void PublishEvent(Guid id, User user)
        {
            var eEvent = _db.SingleById<Event>(id);
            eEvent.Published = DateTimeOffset.Now;
            eEvent.Publisher = user;
            _db.Update(eEvent);

            _nService.NotifyUsers(new Notification
            {
                Message = "Event with name: " + eEvent.Name + " is now published!",
                Subject = "Event" + eEvent.Name + " published"
            }, new List<User>(_db.Fetch<User>()));
            //todo Change to specific users with required roles
        }

        public void RemoveFromSlot(Event eEvent, User user, Slot chosenSlot)
        {
            //SlotSignUp userSignUp;
            //foreach (var slot in eEvent.Roster)
            //{
            //    foreach (var slotSignUp in slot.SignUps)
            //    {
            //        if (slotSignUp.User.Id == user.Id)
            //        {
            //            userSignUp = slotSignUp;

            //        }
            //    }
            //    slot.SignUps.RemoveAt(userSignUp);
            //}

            var slot = _db.SingleById<SlotSignUp>(user.Id);
            slot.Deleted = DateTimeOffset.Now;
            _db.Update(slot);
        }
    }
}