using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChainMarketing.Domain.Entities;

namespace ChainMarketing.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<User> Users => Set<User>();

        
       
        public DbSet<ReferralPath> ReferralPaths { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.ReferredBy)
                .WithMany(u => u.DirectReferrals)
                .HasForeignKey(u => u.ReferredById)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete


            modelBuilder.Entity<ReferralPath>()
         .HasOne(rp => rp.User)
         .WithMany()
         .HasForeignKey(rp => rp.UserId)
         .OnDelete(DeleteBehavior.Restrict); // or NoAction

            modelBuilder.Entity<ReferralPath>()
                .HasOne(rp => rp.Referrer)
                .WithMany()
                .HasForeignKey(rp => rp.ReferrerId)
                .OnDelete(DeleteBehavior.Restrict); // only one allowed to cascade


        }


    }
}
