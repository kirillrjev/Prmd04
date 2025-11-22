## Практическая работа 18. Подходы CodeFirst и DatabaseFirst

## Вариант 1


## ФАЙЛ 1 — Models/Product.cs
namespace Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

## ФАЙЛ 2 — AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=ProductsDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

## ФАЙЛ 3 — Program.cs (CRUD-операции)
using Project;
using Project.Models;

using var db = new AppDbContext();

// --- CREATE ---
var product = new Product
{
    Name = "Laptop",
    Price = 55000
};

db.Products.Add(product);
db.SaveChanges();
Console.WriteLine("Product created.");


// --- READ ---
Console.WriteLine("\nProducts in database:");
foreach (var p in db.Products)
{
    Console.WriteLine($"{p.Id} | {p.Name} | {p.Price}");
}


// --- UPDATE ---
product.Price = 49999;
db.SaveChanges();
Console.WriteLine("\nProduct updated.");


// --- DELETE ---
db.Products.Remove(product);
db.SaveChanges();
Console.WriteLine("\nProduct deleted.");

## Команды миграций (выполнить в папке с проектом)
dotnet ef migrations add InitialCreate
dotnet ef database update