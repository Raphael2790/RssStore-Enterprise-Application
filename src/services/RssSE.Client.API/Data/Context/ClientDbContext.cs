using Microsoft.EntityFrameworkCore;
using RssSE.Client.API.Models;
using RssSE.Core.Data;
using System.Linq;
using System.Threading.Tasks;
using ClientModel = RssSE.Client.API.Models.Client;

namespace RssSE.Client.API.Data.Context
{
    public class ClientDbContext : DbContext, IUnitOfWork
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> options): base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
           
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientDbContext).Assembly);
        }

        public async Task<bool> Commit() => await base.SaveChangesAsync() > 0;
    }
}
