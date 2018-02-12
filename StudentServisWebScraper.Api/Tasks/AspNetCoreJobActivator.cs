using System;
using Hangfire;

namespace StudentServisWebScraper.Api.Tasks
{
    public class AspNetCoreJobActivator : JobActivator
    {
        private IServiceProvider serviceProvider;

        public AspNetCoreJobActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            return serviceProvider.GetService(jobType);
        }
    }
}
