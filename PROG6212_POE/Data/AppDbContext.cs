using Microsoft.EntityFrameworkCore;
using PROG6212_POE.Models;
using System.Collections.Generic;

namespace PROG6212_POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<User> Users { get; set; }
    }
}