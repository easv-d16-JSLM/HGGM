namespace HGGM.Models.Audit
{
    public class LoginAudit : AuditEntryBase
    {
        public string Identity { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public override string Message => $"User '{Name}' logged in using '{Identity}' with '{Provider}' from '{Ip}'";
        public string Provider { get; set; }
    }
}