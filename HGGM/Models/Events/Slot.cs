using System.Collections.Generic;

namespace HGGM.Models.Events
{
    public class Slot
    {
        public ApprovalType CanApprove { get; set; }
        public string Name { get; set; }
        public IList<IRequirement> Requirements { get; set; }
        public IList<SlotSignUp> SignUps { get; set; }
        public IList<Slot> SubSlots { get; set; }
    }
}