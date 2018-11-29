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

        }

        public void EditEvent(Event eEvent)
        {
            _db.Update(eEvent);
            // Insert notification service
        }

        public void DeleteEvent(Guid id)
        {
            _db.SingleById<Event>(id);
            // Insert notification service
        }

        public void PublishEvent(Guid id, User user)
        {
            _db.Update(_db.SingleById<Event>(id));
            // Insert notification service

        }

        public void DeclineEvent()
        {

            // Insert notification service
        }

        public void AddToSlot()
        {

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
