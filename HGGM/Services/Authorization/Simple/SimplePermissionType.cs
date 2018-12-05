namespace HGGM.Services.Authorization.Simple
{
    public partial class SimplePermission
    {
        public enum SimplePermissionType
        {
            Hangfire,
            GetAuditLog,
            GetRoles,
            EditRoles,
            GetTags,
            EditTags,
            GetUsers,
            EditUsers,
            PublishEvents,

        }
    }
}