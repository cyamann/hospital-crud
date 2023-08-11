using Microsoft.EntityFrameworkCore;

namespace Hastane.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Hasta> hasta { get; set; }
        public DbSet<Models.Ziyaretler> Ziyaretler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the schema name for the Ziyaretler entity
            modelBuilder.Entity<Models.Ziyaretler>().ToTable("Ziyaretler");

            // Other configurations for your entities
        }
    }
}
