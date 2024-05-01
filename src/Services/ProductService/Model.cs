using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace ProductService
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }


        public ProductContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Banana" },
                new Product() { Id = 2, Name = "Apple" },
                new Product() { Id = 3, Name = "Peach" }
            );

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("ProductDb");
        }

    }

    public class Product
    {
        public int Id { get; set; }

        public required string Name { get; set; }
    }
}
