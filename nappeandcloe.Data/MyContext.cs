using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace nappeandcloe.Data
{
    public class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}nappeandcloe.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new MyContext(config.GetConnectionString("ConStr"));
        }
    }


    public class MyContext : DbContext
    {
        private string _connectionString;
        public MyContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        //public DbSet<PickUp> PickUps { get; set; }
        public DbSet<Payment> Payments { get; set; }
        //public DbSet<Return> Returns { get; set; }
        //public DbSet<Damage> Damages { get; set; }
        //public DbSet<Lost> Losts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<ProductLabel> ProductLabels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Liner> Liners { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Size> Sizes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            modelBuilder.Entity<ProductLabel>()
                .HasKey(pr => new { pr.ProductId, pr.LabelId });

            modelBuilder.Entity<ProductLabel>()
                .HasOne(p => p.Product)
                .WithMany(pl => pl.ProductLabels)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<ProductLabel>()
                .HasOne(l => l.Label)
                .WithMany(pl => pl.ProductLabels)
                .HasForeignKey(l => l.LabelId);





            //modelBuilder.Entity<ProductSize>()
            //   .HasKey(pr => new { pr.ProductId, pr.SizeId });

            modelBuilder.Entity<ProductSize>()
                .HasOne(p => p.Product)
                .WithMany(ps => ps.ProductSizes)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<ProductSize>()
                .HasOne(s => s.Size)
                .WithMany(ps => ps.ProductSizes)
                .HasForeignKey(s => s.SizeId);






            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductSizeId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(o => o.Order)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(p => p.ProductSize)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(p => p.ProductSizeId);



            



        //    modelBuilder.Entity<Return>()
        //       .HasKey(r => new { r.OrderDetailId, r.ProductId });

        //    modelBuilder.Entity<Return>()
        //        .HasOne(o => o.OrderDetail)
        //        .WithMany(r => r.Returns)
        //        .HasForeignKey(o => o.OrderDetailId);

        //    modelBuilder.Entity<Return>()
        //       .HasOne(p => p.Product)
        //        .WithMany(r => r.Returns)
        //        .HasForeignKey(p => p.ProductId);


        //    modelBuilder.Entity<PickUp>()
        //        .HasKey(pu => new { pu.OrderDetailId, pu.ProductId });

        //    modelBuilder.Entity<PickUp>()
        //        .HasOne(o => o.OrderDetail)
        //        .WithMany(pu => pu.PickUps)
        //        .HasForeignKey(o => o.OrderDetailId);

        //    modelBuilder.Entity<PickUp>()
        //       .HasOne(p => p.Product)
        //        .WithMany(pu => pu.PickUps)
        //        .HasForeignKey(p => p.ProductId);



        //    modelBuilder.Entity<Lost>()
        //      .HasKey(l => new { l.OrderDetailId, l.ProductId });

        //    modelBuilder.Entity<Lost>()
        //        .HasOne(o => o.OrderDetail)
        //        .WithMany(l => l.Losts)
        //        .HasForeignKey(o => o.OrderDetailId);

        //    modelBuilder.Entity<Lost>()
        //       .HasOne(p => p.Product)
        //        .WithMany(l => l.Losts)
        //        .HasForeignKey(p => p.ProductId);



        //    modelBuilder.Entity<Damage>()
        //      .HasKey(d => new { d.OrderDetailId, d.ProductId });

        //    modelBuilder.Entity<Damage>()
        //        .HasOne(o => o.OrderDetail)
        //        .WithMany(d => d.Damages)
        //        .HasForeignKey(o => o.OrderDetailId);

        //    modelBuilder.Entity<Damage>()
        //       .HasOne(p => p.Product)
        //        .WithMany(d => d.Damages)
        //        .HasForeignKey(p => p.ProductId);
        }
    }
}
