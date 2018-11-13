using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HGGM.Services.Authorization.Simple
{
    public class SimplePermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission"))
            {
                var permission = Enum.Parse<SimplePermission>(policyName.Substring("Permission".Length));
                return Task.FromResult(new AuthorizationPolicyBuilder()
                    .AddRequirements(new SimplePermissionRequirement(permission)).Build());
            }

            throw new NotImplementedException();
        }
    }
}