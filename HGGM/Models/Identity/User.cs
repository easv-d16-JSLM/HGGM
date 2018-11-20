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
        public string Name { get; set; }
        public string Headline { get; set; }
        public string Biography { get; set; }
        public string Country { get; set; }
        public Guid Avatar { get; set; }
    }
}
