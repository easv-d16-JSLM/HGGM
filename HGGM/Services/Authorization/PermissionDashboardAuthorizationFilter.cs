using Hangfire.Dashboard;
using HGGM.Services.Authorization.Simple;
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
                .AuthorizeAsync(httpContext.User, null,
                    new SimplePermissionRequirement(
                        new SimplePermission(SimplePermission.SimplePermissionType.Hangfire))).GetAwaiter()
                .GetResult();

            return result.Succeeded;
        }
    }
}