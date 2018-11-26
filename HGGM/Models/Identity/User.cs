using System;
using System.Collections.Generic;
using AspNetCore.Identity.LiteDB.Models;

namespace HGGM.Models.Identity
{
    public class User : ApplicationUser
    {
        public NotificationConfig Config { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<Notification> Notifications { get; set; }
        public string Steam64ID { get; set; }
        public string TeamspeakUID { get; set; }
    }
}