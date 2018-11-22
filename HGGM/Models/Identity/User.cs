using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models.Identity
{
    public class User : AspNetCore.Identity.LiteDB.Models.ApplicationUser
    {
        public DateTime DateOfBirth { get; set; }
        public string Steam64ID { get; set; }
        public string TeamspeakUID { get; set; }

        //TODO: Research many to many in LiteDB

        //[BsonRef(nameof(Tag))]
        //public IList<Tag> Tags { get; set; }
    }
}
