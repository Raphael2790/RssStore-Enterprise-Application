using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssSE.Catalog.API.Models;

namespace RssSE.Catalog.API.Data.EntitiesMappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnName("Description")
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Image)
                .IsRequired()
                .HasColumnName("Image")
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Value)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("Value");

            builder.Property(p => p.Active)
                .HasColumnName("Active")
                .HasColumnType("bit");
        }
    }
}
