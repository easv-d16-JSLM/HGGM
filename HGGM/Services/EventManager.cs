﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Events;
using HGGM.Models.Identity;
using LiteDB;
using Microsoft.AspNetCore.Identity;

namespace HGGM.Services
{
    public class EventManager
    {
        private readonly LiteRepository _db;
        public EventManager(LiteRepository db)
        {
            _db = db;
        }

        public void AddEvent(Event eEvent)
        {
            eEvent.Created = DateTimeOffset.Now;
            _db.Insert(eEvent);
        }

        public void EditEvent(Event eEvent)
        {
            _db.Update<Event>(eEvent);
        }
        
        public void DeleteEvent(Guid id)
        {
            _db.SingleById<Event>(id);
        }

        public void PublishEvent(Guid id, User user)
        {
            _db.Update(_db.SingleById<Event>(id));

        }

        public void DeclineEvent()
        {

        }

        public void AddToSlot()
        {


        }

        public void RemoveFromSlot()
        {

        }

        private bool CanUserJoinSlot()
        {

            return true;
        }

    }
}
