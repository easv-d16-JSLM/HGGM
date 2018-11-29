using System;
using System.Collections.Generic;
using HGGM.Models.Identity;

namespace HGGM.ViewModels
{
    public class EditUserViewModel
    {
        public List<string> CountryList { get; set; }
        public bool RemoveAvatar { get; set; }
        public Dictionary<string, bool> Roles { get; set; }
        public User User { get; set; }
        public string Email { get; set; }
    }
}