using System;
using System.Collections.Generic;
using HGGM.Models.Identity;
using LiteDB;

namespace HGGM.Models.Events
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User Author { get; set; }
        public User Publisher { get; set; }
        public IList<Slot> Roster { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Published { get; set; }
        public DateTimeOffset TakesPlace { get; set; }
    }
}