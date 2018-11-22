﻿using System;
using HGGM.Models.Identity;
using LiteDB;

namespace HGGM.Models.Events
{
    public class SlotSignUp
    {
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Deleted { get; set; }
        public User User { get; set; }
    }
}