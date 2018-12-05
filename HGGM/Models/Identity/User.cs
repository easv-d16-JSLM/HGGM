using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.Identity.LiteDB.Models;
using Microsoft.AspNetCore.Identity;

namespace HGGM.Models.Identity
{
    public class User : ApplicationUser
    {
        public User()
        {
            Email = new EmailInfo();
        }
        public NotificationConfig Config { get; set; }
        public IList<Notification> Notifications { get; set; }
        public string Biography { get; set; }
        [PersonalData] public string Country { get; set; }
        [PersonalData] public DateTime DateOfBirth { get; set; }
        public string Headline { get; set; }
        public DateTime JoinDate { get; set; }
        [PersonalData] public string Name { get; set; }

        [PersonalData] public string TeamspeakUID { get; set; }

        public string Steam64ID => SerializableLogins.SingleOrDefault(l => l.LoginProvider == "Steam")?.ProviderKey?.Split('/')?.Last();

        //TODO: Research many to many in LiteDB

        //[BsonRef(nameof(Tag))]
        //public IList<Tag> Tags { get; set; }
    }
}