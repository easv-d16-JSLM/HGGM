using System.Collections.Generic;
using LiteDB;

namespace HGGM.Models.Events
{
    public class Slot
    {
        public ApprovalType CanApprove { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public IList<IRequirement> Requirements { get; set; }
        public IList<SlotSignUp> SignUps { get; set; }
        public IList<Slot> SubSlots { get; set; }
    }
}