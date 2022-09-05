using CryptoTracker.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Deal>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Deals)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId);
            });

        }
    }
}
