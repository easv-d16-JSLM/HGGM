using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HGGM.Services.Authorization.Simple
{
    public class SimplePermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission"))
            {
                var permission = new SimplePermission(
                    Enum.Parse<SimplePermissionType>(policyName.Substring("Permission".Length)));
                return Task.FromResult(new AuthorizationPolicyBuilder()
                    .AddRequirements(new SimplePermissionRequirement(permission)).Build());
            }

            throw new NotImplementedException();
        }
    }
}