using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace HGGM.Services.Authorization
{
    public class PermissionDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var authorizationService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var result = authorizationService
                .AuthorizeAsync(httpContext.User, null, new PermissionRequirement(Permission.Hangfire)).GetAwaiter()
                .GetResult();

            return result.Succeeded;
        }
    }
}