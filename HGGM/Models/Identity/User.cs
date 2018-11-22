using System;
using AspNetCore.Identity.LiteDB.Models;

namespace HGGM.Models.Identity
{
    public class User : ApplicationUser
    {
        public DateTime DateOfBirth { get; set; }
        public string Steam64ID { get; set; }
        public string TeamspeakUID { get; set; }

        //TODO: Research many to many in LiteDB

        //[BsonRef(nameof(Tag))]
        //public IList<Tag> Tags { get; set; }
    }
}