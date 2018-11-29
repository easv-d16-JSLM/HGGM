using System;

namespace HGGM.Models
{
    public class Notification
    {
        public DateTimeOffset DateNotified { get; set; } = DateTimeOffset.Now;
        public string Link { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool Viewed { get; set; }
    }
}