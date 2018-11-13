using System;
using System.Collections.Generic;
using HGGM.Models.Identity;
using LiteDB;

namespace HGGM.Models.Events
{
    public class Event
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public User Author { get; set; }
        public User Publisher { get; set; }
        [BsonRef(nameof(Slot))] public IList<Slot> Roster { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Published { get; set; }
        public DateTimeOffset TakesPlace { get; set; }
    }

    public enum ApprovalType
    {
        Nobody,
        Children,
        Descendants,
        Everybody
    }

    public class Slot
    {
        public ApprovalType CanApprove { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public IList<IRequirement> Requirements { get; set; }
        public IList<SlotSignUp> SignUps { get; set; }
        public IList<Slot> SubSlots { get; set; }
    }

    public class SlotSignUp
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Deleted { get; set; }
        public ObjectId Id { get; set; }
        public Slot Slot { get; set; }
        public User User { get; set; }
    }

    public interface IRequirement
    {
        //Implement in different classes
    }
}