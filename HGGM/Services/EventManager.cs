using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models;
using HGGM.Models.Events;
using HGGM.Models.Identity;
using HGGM.Services.Authorization.Simple;
using HGGM.Services.Authorization.Tag;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HGGM.Services
{
    public class EventManager
    {
        private readonly LiteRepository _db;
        private readonly INotificationService _nService;
        
        public EventManager(LiteRepository db, INotificationService nService)
        {

            _db = db;
            _nService = nService;
            
        }

        public List<User> GetUsersFromEvent(Event eEvent)
        {
            var rosterList = eEvent.Roster;
            var listOfUsersSignedUp = new List<User>();

            foreach (var slot in rosterList)
            {
                var slotSignUps = slot.SignUps;

                foreach (var user in slotSignUps)
                {
                    listOfUsersSignedUp.Add(user.User);
                }
            }

            return listOfUsersSignedUp;
        }

        public void AddEvent(Event eEvent)
        {
            eEvent.Created = DateTimeOffset.Now;
            _db.Insert(eEvent);
            _nService.NotifyUsers(new Notification()
            {
                Message = "Event with name: " + eEvent.Name + " created! Author: " + eEvent.Author + "Takes place: " +
                          eEvent.TakesPlace,
                Subject = "Event" + eEvent.Name + " awaiting approval"
            }, new List<User>(_db.Fetch<User>()));
            //todo Change to specific users with required roles
        }

        public void EditEvent(Event eEvent)
        {
            _db.Update(eEvent);
            _nService.NotifyUsers(new Notification()
            {
                Message = "Event with name: " + eEvent.Name + " updated!",
                Subject = "Event" + eEvent.Name + " updated"
            }, GetUsersFromEvent(eEvent));
        }

        public void DeleteEvent(Guid id)
        {
            var eEvent = _db.SingleById<Event>(id);
            _db.Delete<Event>(id);
            _nService.NotifyUsers(new Notification()
            {
                Message = "Event with name: " + eEvent.Name + " deleted!",
                Subject = "Event" + eEvent.Name + " deleted"
            }, GetUsersFromEvent(eEvent));

        }

        public void PublishEvent(Guid id, User user)
        {
            var eEvent = _db.SingleById<Event>(id);
            eEvent.Published = DateTimeOffset.Now;
            eEvent.Publisher = user;
            _db.Update(eEvent);

            _nService.NotifyUsers(new Notification()
            {
                Message = "Event with name: " + eEvent.Name + " is now published!",
                Subject = "Event" + eEvent.Name + " published"
            }, new List<User>(_db.Fetch<User>()));
            //todo Change to specific users with required roles

        }

        public void DeclineEvent(Guid id, string declineMessage)
        {
            var eEvent = _db.SingleById<Event>(id);
            var list = new List<User>
            {
                eEvent.Author
            };

            _nService.NotifyUsers(new Notification()
            {
                Message = declineMessage,
                Subject = "Event" + eEvent.Name + " declined"
            }, list);

        }

        public void AddToSlot(Event eEvent, User user, string note, Slot chosenSlot)
        {
            var slotSignUp = new SlotSignUp()
            {
                Created = DateTimeOffset.Now,
                Note = note,
                User = user
            };
            var slot = eEvent.Roster.Equals(chosenSlot);
            if (eEvent.Roster.Contains(chosenSlot))
            {
                
            }

            // Insert notification service
        }

        public void RemoveFromSlot()
        {

            // notify user
        }

        private bool CanUserJoinSlot(HttpContext context)
        {
            var authorizationService = context.RequestServices.GetRequiredService<IAuthorizationService>();
            var result = authorizationService
                .AuthorizeAsync(context.User, null, new TagRequirement()).GetAwaiter()
                .GetResult();

            return result.Succeeded;            
        }

    }
}
