using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssSE.Cart.API.Models;

namespace RssSE.Cart.API.Data.Mappings
{
    public class CartItemMapping : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.Property(c => c.UnitValue)
                .HasColumnType("decimal(12,2)");

            builder.Property(c => c.Image)
                .HasMaxLength(250);
        }
    }
}
