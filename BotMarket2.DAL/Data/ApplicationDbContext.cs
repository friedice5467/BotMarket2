using BotMarket2.Common.Models;
using BotMarket2.DAL.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BotMarket2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<HistoricalStockData> HistoricalStockData { get; set; }
        public DbSet<Simulation> Simulations { get; set; }
        public DbSet<Algorithm> Algorithms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Simulation>()
                .Property(s => s.UserId) 
                .IsRequired(); 

            modelBuilder.Entity<Algorithm>()
                .Property(a => a.UserId) 
                .IsRequired(); 

            modelBuilder.Entity<HistoricalStockData>()
                .HasIndex(h => new { h.Symbol, h.Date });
        }

    }
}
