using System;
using System.Collections.Generic;
using HGGM.Models.Identity;

namespace HGGM.Models.Events
{
    public class Slot
    {
        public string Name { get; set; }
        public IList<IRequirement> Requirements { get; set; }
        public IList<SlotSignUp> SignUps { get; set; }
        public IList<Slot> SubSlots { get; set; }
    }

    public class SlotSignUp
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Deleted { get; set; }
        public Slot Slot { get; set; }
        public User User { get; set; }
    }

    public interface IRequirement
    {
        //Implement in different classes
    }
}