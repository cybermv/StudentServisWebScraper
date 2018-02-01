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
using Hangfire.SQLite;
using StudentServisWebScraper.Api.Tasks;
using StudentServisWebScraper.Api.Data;
using Microsoft.EntityFrameworkCore;

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

            string connStr = this.Configuration.GetConnectionString("StudentServisDatabase");

            services.AddDbContext<StudentServisWebScraperDataContext>(
                options => options.UseSqlite(connStr));

            services.AddHangfire(
                config => config.UseSQLiteStorage(connStr, new SQLiteStorageOptions()));

            services.AddSingleton(Configuration.Get<ScraperConfiguration>());

            services.AddTransient<JobOfferParser>();
            services.AddTransient<JobScraper>();
            services.AddTransient<JobOfferDataManager>();
            services.AddTransient<JobScrapingTask>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            JobScrapingTask.Provider = app.ApplicationServices;
            RecurringJob.AddOrUpdate(
                "JobScrapingTask",
                () => JobScrapingTask.CreateAndExecute(),
                Cron.MinuteInterval(this.Configuration.Get<ScraperConfiguration>().ScrapingIntervalMinutes));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}
