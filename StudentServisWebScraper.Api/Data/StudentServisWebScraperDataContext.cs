using Microsoft.EntityFrameworkCore;

namespace StudentServisWebScraper.Api.Data
{
    /// <summary>
    /// The data context class for this application
    /// </summary>
    public class StudentServisWebScraperDataContext : DbContext
    {
        public StudentServisWebScraperDataContext(DbContextOptions<StudentServisWebScraperDataContext> options)
            : base(options)
        {
        }

        public DbSet<JobOffer> JobOffers { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
