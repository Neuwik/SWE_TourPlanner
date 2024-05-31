using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SWE_TourPlanner_WPF.DataBase
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        private string _connectionString ="Host=localhost;Port=5432;Database=SWE_TourPlanner_DB;Username=SWE_TourPlanner_User;Password=Debian123!;";
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourLog> TourLogs { get; set; }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            *//*if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }*//*
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tour>()
                .HasMany(t => t.TourLogs)
                .WithOne(t => t.Tour)
                .HasForeignKey(t => t.TourId)
                .IsRequired();
        }
    }
}
