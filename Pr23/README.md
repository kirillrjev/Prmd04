## Практическая работа 23. Join, группировка, оптимизация запросов, IAsyncEnumerable и сырые SQL-запросы в EF Core

## Вариант 1

## 1. Модели данных
Customer.cs
namespace Project.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

Order.cs
namespace Project.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

## 2. DbContext

AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=JoinDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

## 3. Join + группировка
using var context = new AppDbContext();

var customerOrders =
    from order in context.Orders
    join customer in context.Customers
    on order.CustomerId equals customer.Id
    group order by customer.FullName into g
    select new
    {
        CustomerName = g.Key,
        TotalOrders = g.Count(),
        TotalAmount = g.Sum(o => o.TotalAmount)
    };

foreach (var item in customerOrders)
{
    Console.WriteLine($"{item.CustomerName} — Заказов: {item.TotalOrders}, Сумма: {item.TotalAmount}");
}

## 4. Оптимизация запросов (Include, Select, AsNoTracking)
var optimized = context.Customers
    .Include(c => c.Orders)
    .Where(c => c.Orders.Any())
    .Select(c => new
    {
        c.FullName,
        OrdersCount = c.Orders.Count,
        Total = c.Orders.Sum(o => o.TotalAmount)
    })
    .AsNoTracking()
    .ToList();

## 5. Асинхронное перечисление IAsyncEnumerable
await foreach (var order in context.Orders.AsAsyncEnumerable())
{
    Console.WriteLine($"{order.Id} — {order.TotalAmount}");
}

## 6. Сырые SQL-запросы
Raw SELECT:
var highOrders = context.Orders
    .FromSqlRaw("SELECT * FROM Orders WHERE TotalAmount > {0}", 500)
    .ToList();

Raw INSERT/UPDATE:
context.Database.ExecuteSqlRaw(
    "UPDATE Orders SET TotalAmount = TotalAmount + 10 WHERE Id = {0}", 1);

## Program.cs (готов)
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

using var context = new AppDbContext();

// JOIN + Group
var customerOrders =
    from order in context.Orders
    join customer in context.Customers
    on order.CustomerId equals customer.Id
    group order by customer.FullName into g
    select new
    {
        CustomerName = g.Key,
        TotalOrders = g.Count(),
        TotalAmount = g.Sum(o => o.TotalAmount)
    };

Console.WriteLine("=== JOIN + GROUP ===");
foreach (var item in customerOrders)
    Console.WriteLine($"{item.CustomerName}: Orders={item.TotalOrders}, Total={item.TotalAmount}");

// Async stream
Console.WriteLine("\n=== IAsyncEnumerable ===");
await foreach (var o in context.Orders.AsAsyncEnumerable())
    Console.WriteLine($"Order {o.Id}: {o.TotalAmount}");

// Raw SQL
Console.WriteLine("\n=== RAW SQL ===");
var highOrders = context.Orders
    .FromSqlRaw("SELECT * FROM Orders WHERE TotalAmount > {0}", 500)
    .ToList();

foreach (var o in highOrders)
    Console.WriteLine($"High Order: {o.Id} | {o.TotalAmount}");