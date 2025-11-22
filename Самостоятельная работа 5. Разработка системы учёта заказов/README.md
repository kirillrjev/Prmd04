## Самостоятельная работа 5. Разработка системы учёта заказов

## 1. Модели данных
Customer.cs
using System.Collections.Generic;

namespace Project.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

Product.cs
namespace Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

Order.cs
using System;
using System.Collections.Generic;

namespace Project.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

OrderItem.cs
namespace Project.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

## 2. DbContext
ShopContext.cs
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ShopContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=OrderSystemDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
        }
    }
}

## 3. Пример CRUD и создание заказа с транзакцией
Program.cs
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

Console.WriteLine("=== Система учёта заказов ===");

using var context = new ShopContext();

context.Database.Migrate();

// Создание клиента
var customer = new Customer
{
    FullName = "John Doe",
    Email = "john@example.com",
    Phone = "1234567890"
};

context.Customers.Add(customer);

## Создание продуктов
var p1 = new Product { Name = "Laptop", Price = 1500, Stock = 10 };
var p2 = new Product { Name = "Mouse", Price = 50, Stock = 100 };
context.Products.AddRange(p1, p2);
await context.SaveChangesAsync();

Console.WriteLine("Создание заказа...");

var order = new Order
{
    CustomerId = customer.Id,
    OrderDate = DateTime.Now,
    TotalAmount = 1550,
    OrderItems = new List<OrderItem>
    {
        new OrderItem { ProductId = p1.Id, Quantity = 1, Price = 1500 },
        new OrderItem { ProductId = p2.Id, Quantity = 1, Price = 50 }
    }
};

using var transaction = await context.Database.BeginTransactionAsync();
try
{
    context.Orders.Add(order);

    p1.Stock -= 1;
    p2.Stock -= 1;

    await context.SaveChangesAsync();
    await transaction.CommitAsync();
    Console.WriteLine("Заказ успешно оформлен.");
}
catch
{
    await transaction.RollbackAsync();
    Console.WriteLine("Ошибка. Транзакция отменена.");
}

## Просмотр заказов
Console.WriteLine("\n=== Список заказов ===");

var orders = context.Orders
    .Include(o => o.Customer)
    .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
    .AsNoTracking()
    .ToList();

foreach (var o in orders)
{
    Console.WriteLine($"\nЗаказ #{o.Id}, Клиент: {o.Customer.FullName}, Сумма: {o.TotalAmount}");

    foreach (var item in o.OrderItems)
        Console.WriteLine($"  - {item.Product.Name}, qty: {item.Quantity}, price: {item.Price}");
}
