using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace OrderService
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }


        public OrderContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Lines)
                .WithOne(ol => ol.Order)
                .HasForeignKey(ol => ol.OrderId)
                .HasPrincipalKey(o => o.Id);

            var order1 = new Order() { CreationDate = new DateTime(2022, 01, 02), Id = 1 };
            var order2 = new Order() { CreationDate = new DateTime(2023, 07, 29), Id = 2 };


            modelBuilder.Entity<Order>().HasData(order1, order2);


            modelBuilder.Entity<OrderLine>().HasData(new OrderLine[] {
                    new OrderLine() { Id = 1, ProductId = 2, OrderId = 1, Quantity = 10 } ,
                    new OrderLine() { Id = 2, ProductId = 3, OrderId = 1, Quantity = 3 },
                      new OrderLine() { Id = 3, ProductId = 1, OrderId = 2, Quantity =1 } ,
                    new OrderLine() { Id = 4, ProductId = 2, OrderId = 2, Quantity = 1 }
            });

         
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("OrderDb");
        }

    }

    public class Order
    {
        public int Id { get; set; }

        public required DateTime CreationDate { get; set; }

        public ICollection<OrderLine>? Lines { get; set; }
    }

    public class OrderLine
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public int OrderId { get; set; }

        public Order? Order { get; set; }
    }
}
