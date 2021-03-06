﻿using System;
using System.Collections.Generic;
using HGGM.Models.Identity;

namespace HGGM.Models.Events
{
    public class Event
    {
        public User Author { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid Id { get; set; }
        //todo Create evnetype class for templates and stuff
        public string EventType { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Published { get; set; }
        public User Publisher { get; set; }
        public IList<Slot> Roster { get; set; }
        public DateTimeOffset TakesPlace { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}