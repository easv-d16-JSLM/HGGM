namespace HGGM.Models
{
    public class NotificationConfig
    {
        /// <summary>
        ///     Enable in-app notification
        /// </summary>
        public bool AccountNotify { get; set; }

        /// <summary>
        ///     Enable email notification
        /// </summary>
        public bool EmailNotify { get; set; }
    }
}