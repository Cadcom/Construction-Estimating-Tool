using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CET.Shared;
using CET.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace CET.Data
{
    public class CETDBContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Estimate> Estimates { get; set; }

        public CETDBContext(DbContextOptions<CETDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estimate>()
           .HasOne(p => p.Account)
           .WithMany(b => b.Estimates)
           .HasForeignKey(p => p.AccountId)
           .HasPrincipalKey(b => b.ID);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(CETDBContext).Assembly);
        }
    }
}
