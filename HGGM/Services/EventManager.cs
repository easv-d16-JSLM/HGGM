using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HGGM.Models;
using HGGM.Models.Events;
using HGGM.Models.Identity;
using HGGM.Services.Authorization.Simple;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace HGGM.Services
{
    public class EventManager
    {
        private readonly IAuthorizationService _authorization;
        private readonly LiteRepository _db;
        private readonly IUserClaimsPrincipalFactory<User> _factory;
        private readonly IStringLocalizer<EventManager> _localizer;
        private readonly INotificationService _nService;

        public EventManager(LiteRepository db, INotificationService nService, IStringLocalizer<EventManager> localizer,
            IAuthorizationService authorization, IUserClaimsPrincipalFactory<User> factory)
        {
            _db = db;
            _nService = nService;
            _localizer = localizer;
            _authorization = authorization;
            _factory = factory;
        }

        public async Task AddEvent(Event eEvent)
        {
            // Create & insert event into DB
            eEvent.Created = DateTimeOffset.Now;
            _db.Insert(eEvent);

            // Notify Users
            var list = await PermissionCheck(SimplePermissionType.PublishEvents);
            await _nService.NotifyUsers(new Notification
            {
                Message = _localizer["createEventMessage", eEvent.Author, eEvent.TakesPlace],
                Subject = _localizer["createEventSubject", eEvent.Name]
            }, list);
        }

        public void AddToSlot(Event eEvent, User user, string note, Slot chosenSlot)
        {
            // Create SignUp to be inserted into event.
            var slotSignUp = new SlotSignUp
            {
                Created = DateTimeOffset.Now,
                Note = note,
                User = user
            };

            // Updates DB with new SignUp. 
            var spot = eEvent.Roster.IndexOf(chosenSlot); // Test to see if .IndexOf finds the indext of slot. 
            chosenSlot.SignUps.Add(slotSignUp);
            eEvent.Roster.Insert(spot, chosenSlot); // Test to see if .Insert removes the spot it is inserted to.
            _db.Update(eEvent);

            // Notify users involved
            var list = new List<User>
            {
                eEvent.Author,
                user
            };
            _nService.NotifyUsers(new Notification
            {
                Message = _localizer["signUpMessage", user.Name, chosenSlot.Name],
                Subject = _localizer["signUpSubject", eEvent.Name]
            }, list);
        }

        public async Task<bool> CanUserJoinSlot(Slot slot, User user)
        {
            if (slot.AmountOfSignup == 0 || slot.AmountOfSignup < slot.SignUps.Count) return false;
            var claimsPrincipal = await _factory.CreateAsync(user);

            foreach (var requirement in slot.Requirements)
            {
                var result = (await _authorization.AuthorizeAsync(claimsPrincipal, null,
                    requirement)).Succeeded;
                if (result == false) return false;
            }

            return true;
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
                Subject = _localizer["declineEventSubject", eEvent.Name]
            }, list);
        }

        public void DeleteEvent(Guid id, User userWhoDeleted)
        {
            var eEvent = _db.SingleById<Event>(id);
            _db.Delete<Event>(id);
            _nService.NotifyUsers(new Notification
            {
                Message = _localizer["deleteEventMessage", userWhoDeleted.Name],
                Subject = _localizer["deleteEventSubject", eEvent.Name]
            }, GetUsersFromEvent(eEvent));
        }

        public void EditEvent(Event eEvent, User UserWhoEdited)
        {
            _db.Update(eEvent);
            _nService.NotifyUsers(new Notification
            {
                Message = _localizer["editEventMessage", UserWhoEdited.Name],
                Subject = _localizer["editEventSubject", eEvent.Name]
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

        private async Task<List<User>> PermissionCheck(SimplePermissionType type)
        {
            var list = new List<User>();

            foreach (var user in _db.Fetch<User>())
            {
                var claimsPrincipal = await _factory.CreateAsync(user);
                var notify = (await _authorization.AuthorizeAsync(claimsPrincipal, null,
                    SimplePermissionRequirement.For(type))).Succeeded;
                if (notify) list.Add(user);
            }

            return list;
        }

        public void PublishEvent(Guid id, User user)
        {
            var eEvent = _db.SingleById<Event>(id);
            eEvent.Published = DateTimeOffset.Now;
            eEvent.Publisher = user;
            _db.Update(eEvent);

            _nService.NotifyUsers(new Notification
            {
                Message = _localizer["publishEventMessage", eEvent.Name, eEvent.Published],
                Subject = _localizer["publishEventSubject", eEvent.Name]
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