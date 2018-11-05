using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Formatting.Json;

namespace HGGM.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute( Permission permission) : base($"Permission{permission.ToString()}")
        {
        }
    }
}
