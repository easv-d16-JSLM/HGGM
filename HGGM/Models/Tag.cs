using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGGM.Models.Identity;

namespace HGGM.Models
{
    public class Tag
    {
        public string TagId { get; set; }
        public List<User> Users { get; set; }
        public string TagName { get; set; }

        public Tag(string tagId, List<User> users, string tagName)
        {
            TagId = tagId;
            Users = users;
            TagName = tagName;
        }     
    }
}
