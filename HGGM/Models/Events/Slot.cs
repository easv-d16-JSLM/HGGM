using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HGGM.Models.Events
{
    public class Slot
    {
        public ApprovalType CanApprove { get; set; }
        public string Name { get; set; }
        public IList<IAuthorizationRequirement> Requirements { get; set; }
        public IList<SlotSignUp> SignUps { get; set; }
        public IList<Slot> SubSlots { get; set; }
    }
}