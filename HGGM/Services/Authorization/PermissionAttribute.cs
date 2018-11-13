using System;
using Microsoft.AspNetCore.Authorization;

namespace HGGM.Services.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(Permission permission) : base($"Permission{permission.ToString()}")
        {
        }
    }
}