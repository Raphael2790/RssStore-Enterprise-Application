﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RssSE.Cart.API.Data;

namespace RssSE.Cart.API.Migrations
{
    [DbContext(typeof(CartDbContext))]
    partial class CartDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RssSE.Cart.API.Models.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitValue")
                        .HasColumnType("decimal(12,2)");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("RssSE.Cart.API.Models.CustomerCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(10,2)");

                    b.Property<bool>("VoucherApplyed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .HasName("IDX_Client");

                    b.ToTable("ClientsCarts");
                });

            modelBuilder.Entity("RssSE.Cart.API.Models.CartItem", b =>
                {
                    b.HasOne("RssSE.Cart.API.Models.CustomerCart", "ClientCart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .IsRequired();
                });

            modelBuilder.Entity("RssSE.Cart.API.Models.CustomerCart", b =>
                {
                    b.OwnsOne("RssSE.Cart.API.Models.Voucher", "Voucher", b1 =>
                        {
                            b1.Property<Guid>("CustomerCartId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnName("VoucherCode")
                                .HasColumnType("varchar(50)");

                            b1.Property<decimal?>("DiscountValue")
                                .HasColumnName("DiscountValue")
                                .HasColumnType("decimal(10,2)");

                            b1.Property<decimal?>("Percentage")
                                .HasColumnName("Percentage")
                                .HasColumnType("decimal(10,2)");

                            b1.Property<int>("VoucherType")
                                .HasColumnName("DiscountType")
                                .HasColumnType("int");

                            b1.HasKey("CustomerCartId");

                            b1.ToTable("ClientsCarts");

                            b1.WithOwner()
                                .HasForeignKey("CustomerCartId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
