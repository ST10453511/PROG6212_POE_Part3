using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;

namespace PROG6212_POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; }

        // FIX FOR DECIMAL WARNINGS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tell SQL Server to use 18 digits total, with 2 decimal places (Standard for Money)
            modelBuilder.Entity<User>()
                .Property(u => u.HourlyRate)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Claim>()
                .Property(c => c.HourlyRate)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Claim>()
                .Property(c => c.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Claim>()
                .Property(c => c.HoursWorked)
                .HasColumnType("decimal(18,2)");
        }
    }
}