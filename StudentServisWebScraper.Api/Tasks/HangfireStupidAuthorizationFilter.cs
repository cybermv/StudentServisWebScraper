using Hangfire.Dashboard;
using Hangfire.Annotations;

namespace StudentServisWebScraper.Api.Tasks
{
    public class HangfireStupidAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            // top-notch security
            return context.Request.GetQuery("sudo") == "true";
        }
    }
}
