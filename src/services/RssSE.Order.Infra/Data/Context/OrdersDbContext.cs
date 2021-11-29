using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using RssSE.Core.Data;
using RssSE.Core.Mediator;
using RssSE.Core.Messages;
using RssSE.Order.Domain.Entities;
using RssSE.Order.Infra.Data.Extensions;
using System.Linq;
using System.Threading.Tasks;

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

        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetProperties()
            .Where(x => x.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");
            
            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
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
