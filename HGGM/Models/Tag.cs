using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;
using LiteDB;

namespace HGGM.Models
{

    public class Tag
    {
        public Guid Id { get; set; }
        [BsonRef("users")]
        public List<User> Users { get; set; }
        public string TagName { get; set; }    
    }
}
