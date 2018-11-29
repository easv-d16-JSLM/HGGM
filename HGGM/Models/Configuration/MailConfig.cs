namespace HGGM.Models.Configuration
{
    public class MailConfig
    {
        public string FromEmail { get; set; }

        public string FromName { get; set; }

        // Security to-be-added
        public string Password { get; set; }
        public int Port { get; set; }
        public string SMTPServer { get; set; }
    }
}