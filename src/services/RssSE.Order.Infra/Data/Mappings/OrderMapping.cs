using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RssSE.Order.Infra.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Domain.Entities.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.OwnsOne(o => o.Address, a =>
            {
                a.Property(x => x.Street)
                    .HasColumnName("Street");

                a.Property(x => x.Number)
                    .HasColumnName("Number");

                a.Property(x => x.Complement)
                    .HasColumnName("Complement");

                a.Property(x => x.Neighborhood)
                    .HasColumnName("Neighborhood");

                a.Property(x => x.ZipCode)
                    .HasColumnName("ZipCode");

                a.Property(x => x.City)
                    .HasColumnName("City");

                a.Property(x => x.State)
                    .HasColumnName("State");
            });

            builder.Property(o => o.Discount)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.TotalValue)
                .HasColumnType("decimal(10,2)");

            builder.Property(o => o.Code)
                .HasDefaultValueSql("NEXT VALUE FOR MySequence");

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(o => o.OrderId);
        }
    }
}
