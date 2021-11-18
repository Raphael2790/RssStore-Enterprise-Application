using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssSE.Core.DomainObjects.ValueObjects;
using ClientModel = RssSE.Client.API.Models.Client;

namespace RssSE.Client.API.Data.Mappings
{
    public class ClientMapping : IEntityTypeConfiguration<ClientModel>
    {
        public void Configure(EntityTypeBuilder<ClientModel> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(c => c.Cpf, tf =>
            {
                tf.Property(c => c.Number)
                    .IsRequired()
                    .HasMaxLength(Cpf.CPF_MAX_LENGTH)
                    .HasColumnName("Cpf")
                    .HasColumnType($"varchar({Cpf.CPF_MAX_LENGTH})");
            });

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Address)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.ADDRESS_MAX_LENGTH})");
            });

            // 1 : 1 => Aluno : Endereco
            builder.HasOne(c => c.Address)
                .WithOne(c => c.Client);

            builder.ToTable("Clients");
        }
    }
}
