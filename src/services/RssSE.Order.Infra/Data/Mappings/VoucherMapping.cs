using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssSE.Order.Domain.Entities;

namespace RssSE.Order.Infra.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(v => v.DiscountValue)
                .HasColumnType("decimal(10,2)");

            builder.Property(v => v.Percentage)
                .HasColumnType("decimal(10,2)");
        }
    }
}
