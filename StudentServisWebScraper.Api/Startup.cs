using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentServisWebScraper.Api.Scraping;
using Hangfire;
using Hangfire.SQLite;
using StudentServisWebScraper.Api.Tasks;
using StudentServisWebScraper.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Hangfire.Dashboard;
using StudentServisWebScraper.Api.Authentication;
using Microsoft.AspNetCore.Identity;

namespace StudentServisWebScraper.Api
{
    public class Startup
    {
        /// <summary>
        /// Configuration building
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("scraper.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"scraper.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public const string ConnectionString_SQLServerAzure = "StudentServisDatabaseSQLServerAzure";
        public const string ConnectionString_SQLServerLocal = "StudentServisDatabaseSQLServerLocal";
        public const string ConnectionString_Sqlite = "StudentServisDatabaseSQLite";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddSingleUserStores(this.Configuration.GetSection("SingleUserStore"));

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "SSWS";
                config.LoginPath = "/Application/Login";
                config.LogoutPath = "/Application/Logout";
            });

            services.AddMvc();

            ConfigureData(services);

            ConfigureSSWSServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new IDashboardAuthorizationFilter[] { new HangfireAuthenticationFilter() },
                AppPath = "/Application/Login"
            });
            app.UseHangfireServer();

            GlobalConfiguration.Configuration.UseActivator(new AspNetCoreJobActivator(app.ApplicationServices));

            RecurringJob.AddOrUpdate<JobScrapingTask>(
                "JobScrapingTask",
                jst => jst.Execute(),
                Cron.MinuteInterval(Configuration.Get<ScraperConfiguration>().ScrapingIntervalMinutes));
            
            if (env.IsDevelopment() || bool.Parse(Configuration["UseDeveloperExceptionPage"] ?? "false"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }

        private void ConfigureData(IServiceCollection services)
        {
            string chosenConnectionString = Configuration["ConnectionStringToUse"];
            string connectionString = Configuration.GetConnectionString(chosenConnectionString);

            switch (chosenConnectionString)
            {
                case ConnectionString_SQLServerAzure:
                case ConnectionString_SQLServerLocal:
                    services.AddDbContext<StudentServisWebScraperDataContext>(
                        o => o.UseSqlServer(connectionString));
                    services.AddHangfire(
                        c => c.UseStorage(new NoDeleteSqlServerStorage(connectionString)));
                    break;

                case ConnectionString_Sqlite:
                    services.AddDbContext<StudentServisWebScraperDataContext>(
                        o => o.UseSqlite(connectionString));
                    services.AddHangfire(
                        c => c.UseSQLiteStorage(connectionString));
                    break;
                default:
                    throw new Exception($"Cannot start application without valid connection string! Given value: {chosenConnectionString}.");
            }
        }

        private void ConfigureSSWSServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration.Get<ScraperConfiguration>());

            services.AddTransient<JobOfferParser>();
            services.AddTransient<JobScraper>();
            services.AddTransient<JobOfferDataManager>();
            services.AddTransient<JobScrapingTask>();
        }
    }
}
