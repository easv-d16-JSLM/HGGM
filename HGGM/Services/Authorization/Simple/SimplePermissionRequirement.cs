using Microsoft.AspNetCore.Authorization;

namespace HGGM.Services.Authorization.Simple
{
    public class SimplePermissionRequirement : IAuthorizationRequirement
    {
        public SimplePermissionRequirement(SimplePermission permission)
        {
            Permission = permission;
        }

        public SimplePermission Permission { get; }

        public static SimplePermissionRequirement For(SimplePermissionType permission)
        {
            return new SimplePermissionRequirement(new SimplePermission(permission));
        }
    }
}