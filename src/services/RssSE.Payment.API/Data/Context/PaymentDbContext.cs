using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using RssSE.Core.Data;
using RssSE.Core.Messages;
using RssSE.Payment.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Payment.API.Data.Context
{
    public class PaymentDbContext : DbContext, IUnitOfWork
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) 
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
        }

        public DbSet<Models.Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public async Task<bool> Commit() => await SaveChangesAsync() > 0;
    }
}
