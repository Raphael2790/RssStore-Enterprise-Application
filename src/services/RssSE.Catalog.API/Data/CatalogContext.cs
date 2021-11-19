using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using RssSE.Catalog.API.Models;
using RssSE.Core.Data;
using RssSE.Core.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace RssSE.Catalog.API.Data
{
    public class CatalogContext : DbContext, IUnitOfWork
    {
        public CatalogContext(DbContextOptions<CatalogContext> options): base(options){}

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogContext).Assembly);
        }

        public async Task<bool> Commit() => await base.SaveChangesAsync() > 0;
    }
}
