using Microsoft.EntityFrameworkCore;
using sampleAccount.Helpers;
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

            modelBuilder.Entity<AccountEntity>()
                .HasData(
                    new AccountEntity
                    {
                        Id = new Guid("E74EA75C-EF51-40E1-BD83-086805F32060"),
                        Balance = 0,
                        IBAN = "MOCKACCOUNT1",
                        OwenerId = "SYSTEM",
                        CreatedAt = SystemDateTime.UtcNow()
                    },
                    new AccountEntity
                    {
                        Id = new Guid("5ccc827f-63e3-4bcc-9826-487088444106"),
                        Balance = 0,
                        IBAN = "MOCKACCOUNT2",
                        OwenerId = "SYSTEM",
                        CreatedAt = SystemDateTime.UtcNow()
                    }
                );

            modelBuilder.Entity<TransactionEntity>()
                .HasOne(b => b.From)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.FromId);

            //modelBuilder.ApplyConfiguration(new ManufacturerConfiguration());
            //modelBuilder.ApplyConfiguration(new VehicleModelConfiguration());
        }
    }
}
