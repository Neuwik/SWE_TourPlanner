using Microsoft.EntityFrameworkCore;
using SWE_TourPlanner_WPF.Models;

namespace SWE_TourPlanner_WPF.DataAccessLayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourLog> TourLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tour>().ToTable("Tours");
            modelBuilder.Entity<TourLog>().ToTable("TourLogs");

            modelBuilder.Entity<TourLog>()
                .HasOne(t => t.Tour)
                .WithMany(t => t.TourLogs)
                .HasForeignKey(t => t.TourId);
        }
    }
}
