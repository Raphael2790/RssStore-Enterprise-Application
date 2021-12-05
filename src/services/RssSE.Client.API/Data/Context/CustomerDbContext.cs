using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using RssSE.Client.API.Extensions;
using RssSE.Client.API.Models;
using RssSE.Core.Data;
using RssSE.Core.Mediator;
using RssSE.Core.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Client.API.Data.Context
{
    public class CustomerDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediator;
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IMediatorHandler mediator) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            _mediator = mediator;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();
           
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        }

        public async Task<bool> Commit() 
        {
            var success = await base.SaveChangesAsync() > 0;
            if (success)
                await _mediator.PublishEvents(this);
            return success;
        } 
    }
}
