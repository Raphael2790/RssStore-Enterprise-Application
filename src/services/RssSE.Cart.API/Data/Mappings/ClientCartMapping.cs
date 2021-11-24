using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssSE.Cart.API.Models;

namespace RssSE.Cart.API.Data.Mappings
{
    public class ClientCartMapping : IEntityTypeConfiguration<CustomerCart>
    {
        public void Configure(EntityTypeBuilder<CustomerCart> builder)
        {
            builder.ToTable("ClientsCarts");

            builder.HasIndex(c => c.ClientId)
                .HasName("IDX_Client");

            builder.Property(c => c.TotalValue)
                .HasColumnType("decimal(10,2)");

            builder.HasMany(c => c.CartItems)
                .WithOne(c => c.ClientCart)
                .HasForeignKey(c => c.CartId);
        }
    }
}
