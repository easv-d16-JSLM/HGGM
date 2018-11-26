using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGGM.Models
{
    public class Notification
    {
        public DateTimeOffset DateNotified { get; set; }
        public bool Viewed { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
    }
}
