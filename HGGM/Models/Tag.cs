using System;
using System.Collections.Generic;
using HGGM.Models.Identity;
using LiteDB;

namespace HGGM.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }

        [BsonRef("users")] public List<User> Users { get; set; }
    }
}