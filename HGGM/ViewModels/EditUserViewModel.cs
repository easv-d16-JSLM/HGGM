using System;
using System.Collections.Generic;

namespace HGGM.ViewModels
{
    public class EditUserViewModel
    {
        public bool RemoveAvatar { get; set; }
        public string Biography { get; set; }
        public string Country { get; set; }
        public List<string> CountryList { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Headline { get; set; }
        public string Id { get; set; }
        public DateTime JoinDate { get; set; }
        public string Name { get; set; }
        public Dictionary<string, bool> Roles { get; set; }
        public string Steam64ID { get; set; }
        public string TeamspeakUID { get; set; }
        public string Username { get; set; }
    }
}