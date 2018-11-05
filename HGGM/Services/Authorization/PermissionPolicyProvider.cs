using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HGGM.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission"))
            {
                var permission = Enum.Parse<Permission>(policyName.Substring("Permission".Length));
                return Task.FromResult(new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(permission)).Build());
            }
            throw new NotImplementedException();
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            throw new NotImplementedException();
        }
    }
}
