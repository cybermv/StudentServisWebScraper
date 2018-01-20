using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentServisWebScraper.Api.Scraping;
using Hangfire;
using StudentServisWebScraper.Api.Tasks;
using StudentServisWebScraper.Api.Data;

namespace StudentServisWebScraper.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(Configuration.Get<ScraperConfiguration>());

            services.AddTransient<JobOfferParser>();
            services.AddTransient<JobScraper>();
            //services.AddTransient<JobOfferStorage>();
            services.AddTransient<JobScrapingTask>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*GlobalConfiguration.Configuration.UseSqlServerStorage(
                this.Configuration.GetConnectionString("StudentServisDatabase"));

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(
                "JobScrapingTask",
                () => JobScrapingTask.CreateAndExecute(app.ApplicationServices),
                Cron.MinuteInterval(this.Configuration.Get<ScraperConfiguration>().ScrapingIntervalMinutes));
                */
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
