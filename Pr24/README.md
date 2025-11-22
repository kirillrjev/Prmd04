## Практическая работа 24. Транзакции и паттерн Unit of Work в EF Core 

## Вариант 1

## 1. Модели данных
Product.cs
namespace Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

Order.cs
namespace Project.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
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
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=UnitOfWorkDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

## 3. Репозитории
Общий интерфейс
IRepository.cs
using System.Collections.Generic;

namespace Project.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T entity);
        void Remove(T entity);
    }
}

Интерфейсы для сущностей
IProductRepository.cs
using Project.Models;

namespace Project.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}

IOrderRepository.cs
using Project.Models;

namespace Project.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
    }
}

Реализации репозиториев
ProductRepository.cs
using Project.Models;

namespace Project.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Product product) => _context.Products.Add(product);

        public Product Get(int id) => _context.Products.Find(id);

        public IEnumerable<Product> GetAll() => _context.Products.ToList();

        public void Remove(Product product) => _context.Products.Remove(product);
    }
}

OrderRepository.cs
using Project.Models;

namespace Project.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Order order) => _context.Orders.Add(order);

        public Order Get(int id) => _context.Orders.Find(id);

        public IEnumerable<Order> GetAll() => _context.Orders.ToList();

        public void Remove(Order order) => _context.Orders.Remove(order);
    }
}

## 4. Unit of Work
UnitOfWork.cs
using Project.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Project.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Products = new ProductRepository(_context);
            Orders = new OrderRepository(_context);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}

## 5. Program.cs (полностью готовый пример с транзакцией)
using Project.Data;
using Project.Models;

Console.WriteLine("=== Unit of Work + Transaction Demo ===");

using var unitOfWork = new UnitOfWork(new AppDbContext());

await unitOfWork.BeginTransactionAsync();

try
{
    var product = new Product { Name = "Keyboard", Price = 100 };
    unitOfWork.Products.Add(product);

    var order = new Order { CustomerId = 1, TotalAmount = 100 };
    unitOfWork.Orders.Add(order);

    await unitOfWork.CommitAsync();
    Console.WriteLine("Транзакция успешно завершена.");
}
catch (Exception ex)
{
    await unitOfWork.RollbackAsync();
    Console.WriteLine($"Ошибка: {ex.Message}. Транзакция откатана.");
}