using Microsoft.EntityFrameworkCore;

namespace StudentServisWebScraper.Api.Data
{
    /// <summary>
    /// The data context class for this application
    /// </summary>
    public class StudentServisWebScraperDataContext : DbContext
    {
        public StudentServisWebScraperDataContext()
            : base()
        {
        }

        public StudentServisWebScraperDataContext(DbContextOptions<StudentServisWebScraperDataContext> options)
            : base(options)
        {
        }

        public DbSet<JobOffer> JobOffers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=SSWS.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
