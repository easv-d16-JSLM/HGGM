using System.Collections.Generic;
using HGGM.Models.Identity;

namespace HGGM.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public Dictionary<string, bool> Roles { get; set; }
        public string Username { get; set; }
    }
}