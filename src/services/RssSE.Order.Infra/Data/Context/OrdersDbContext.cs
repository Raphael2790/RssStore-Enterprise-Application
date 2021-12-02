using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using RssSE.Core.Data;
using RssSE.Core.Mediator;
using RssSE.Core.Messages;
using RssSE.Order.Domain.Entities;
using RssSE.Order.Infra.Data.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using CustomerOrder = RssSE.Order.Domain.Entities.Order;

namespace RssSE.Order.Infra.Data.Context
{
    public class OrdersDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediator;
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options, 
                                IMediatorHandler mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<CustomerOrder> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties()
            .Where(x => x.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");
            
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);

            foreach (var relationShip in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
                relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.Entity.GetType().GetProperty("RegisterDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("RegisterDate").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("RegisterDate").IsModified = false;
            }

            var success = await base.SaveChangesAsync() > 0;
            if (success)
                await _mediator.PublishEvents(this);
            return success;
        }
    }
}
