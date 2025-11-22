## Практическая работа 20. Создание моделей данных в Entity Framework

## Вариант 1

## ФАЙЛ 1 — Models/Product.cs (с Data Annotations)
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(0, 999999)]
        public decimal Price { get; set; }
    }
}

## ФАЙЛ 2 — Data/AppDbContext.cs (с Fluent API)
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=ProductsModelDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);
        }
    }
}

## ФАЙЛ 3 — Program.cs (CRUD)
using Project.Data;
using Project.Models;

using var db = new AppDbContext();

// --- CREATE ---
var product = new Product
{
    Name = "Laptop",
    Price = 55999.99m
};
db.Products.Add(product);
db.SaveChanges();
Console.WriteLine("Product added.");

// --- READ ---
Console.WriteLine("\nProducts list:");
foreach (var p in db.Products)
{
    Console.WriteLine($"{p.Id} | {p.Name} | {p.Price}");
}

// --- UPDATE ---
product.Price = 49999.99m;
db.SaveChanges();
Console.WriteLine("\nProduct updated.");

// --- DELETE ---
db.Products.Remove(product);
db.SaveChanges();
Console.WriteLine("\nProduct deleted.");