using Hangfire.Dashboard;
using Hangfire.Annotations;

namespace StudentServisWebScraper.Api.Authentication
{
    public class HangfireAuthenticationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return context.GetHttpContext().User.Identity.IsAuthenticated;
        }
    }
}
