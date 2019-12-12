using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleAccount.DAL.Data
{
    public class DataDbContext: DbContext
    {
        public virtual DbSet<AccountEntity> Accounts { get; set; }
        public virtual DbSet<TransactionEntity> Transactions { get; set; }

        public DataDbContext(DbContextOptions<DataDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>()
                .HasMany(b => b.Transactions)
                .WithOne(e => e.From);

            modelBuilder.Entity<TransactionEntity>()
                .HasOne(b => b.From)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.FromId);

            //modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());
            //modelBuilder.ApplyConfiguration(new VehicleModelConfiguration());
        }
    }
}
