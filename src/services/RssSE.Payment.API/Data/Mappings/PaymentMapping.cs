using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RssSE.Payment.API.Data.Mappings
{
    public class PaymentMapping : IEntityTypeConfiguration<Models.Payment>
    {
        public void Configure(EntityTypeBuilder<Models.Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Transactions)
                .WithOne(t => t.Payment)
                .HasForeignKey(t => t.PaymentId);

            builder.Ignore(p => p.CreditCard);
        }
    }
}
