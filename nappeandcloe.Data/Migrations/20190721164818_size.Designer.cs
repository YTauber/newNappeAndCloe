﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using nappeandcloe.Data;

namespace nappeandcloe.Data.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20190721164818_size")]
    partial class size
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("nappeandcloe.Data.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<bool>("TaxExemt");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("nappeandcloe.Data.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("nappeandcloe.Data.Liner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Cahrge");

                    b.Property<int>("Damages");

                    b.Property<int>("Losts");

                    b.Property<decimal>("MyCaharge");

                    b.Property<int>("PickUps");

                    b.Property<int>("Quantity");

                    b.Property<int>("ReturnedNotUsed");

                    b.Property<int>("Returns");

                    b.HasKey("Id");

                    b.ToTable("Liners");
                });

            modelBuilder.Entity("nappeandcloe.Data.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("DeliveryCharge");

                    b.Property<decimal>("Discount");

                    b.Property<int?>("LinerId");

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<bool>("TaxExemt");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("nappeandcloe.Data.OrderDetail", b =>
                {
                    b.Property<int>("OrderId");

                    b.Property<int>("ProductSizeId");

                    b.Property<int>("Damages");

                    b.Property<int>("Id");

                    b.Property<int>("Losts");

                    b.Property<int>("PickUps");

                    b.Property<decimal>("PricePer");

                    b.Property<int>("Quantity");

                    b.Property<int>("ReturnedNotUsed");

                    b.Property<int>("Returns");

                    b.HasKey("OrderId", "ProductSizeId");

                    b.HasIndex("ProductSizeId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("nappeandcloe.Data.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<int>("OrderId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("nappeandcloe.Data.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Notes");

                    b.Property<string>("PictureName");

                    b.Property<decimal>("Price");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("nappeandcloe.Data.ProductLabel", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<int>("LabelId");

                    b.HasKey("ProductId", "LabelId");

                    b.HasIndex("LabelId");

                    b.ToTable("ProductLabels");
                });

            modelBuilder.Entity("nappeandcloe.Data.ProductSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<string>("Size");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSizes");
                });

            modelBuilder.Entity("nappeandcloe.Data.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("nappeandcloe.Data.Order", b =>
                {
                    b.HasOne("nappeandcloe.Data.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("nappeandcloe.Data.OrderDetail", b =>
                {
                    b.HasOne("nappeandcloe.Data.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("nappeandcloe.Data.ProductSize", "ProductSize")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductSizeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("nappeandcloe.Data.Payment", b =>
                {
                    b.HasOne("nappeandcloe.Data.Order", "Order")
                        .WithMany("Payments")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("nappeandcloe.Data.ProductLabel", b =>
                {
                    b.HasOne("nappeandcloe.Data.Label", "Label")
                        .WithMany("ProductLabels")
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("nappeandcloe.Data.Product", "Product")
                        .WithMany("ProductLabels")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("nappeandcloe.Data.ProductSize", b =>
                {
                    b.HasOne("nappeandcloe.Data.Product", "Product")
                        .WithMany("ProductSizes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
