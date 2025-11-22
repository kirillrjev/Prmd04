## Практическая работа 25. Уровни изоляции и стратегии работы с конкурентным доступом в EF Core

## Вариант 1

## 1. Модель Product с оптимистической блокировкой
Product.cs
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Используется для оптимистической блокировки
        [Timestamp]
        public byte[] RowVersion { get; set; }
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=EFCoreConcurrencyDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}


## 4. Program.cs ― полный пример
Этот код:

Загружает продукт в контекст А

Загружает тот же продукт в контекст B

Контекст А обновляет продукт первым — успешно

Контекст B пытается обновить старую версию → ошибка DbUpdateConcurrencyException

Program.cs
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

Console.WriteLine("=== Оптимистическая блокировка в EF Core ===");

// Контекст A
using var contextA = new AppDbContext();
var productA = await contextA.Products.FirstAsync(p => p.Id == 1);
Console.WriteLine($"Контекст A загрузил продукт. Price = {productA.Price}");

// Контекст B
using var contextB = new AppDbContext();
var productB = await contextB.Products.FirstAsync(p => p.Id == 1);
Console.WriteLine($"Контекст B загрузил продукт. Price = {productB.Price}");

// A меняет цену
productA.Price += 100;
await contextA.SaveChangesAsync();
Console.WriteLine("Контекст A успешно сохранил изменения.");

// B пытается сохранить старую версию
productB.Price += 200;

try
{
    await contextB.SaveChangesAsync();
    Console.WriteLine("Контекст B сохранил изменения (не должно произойти).");
}
catch (DbUpdateConcurrencyException)
{
    Console.WriteLine("❌ Конфликт! Контекст B пытался обновить устаревшие данные.");
}

## Результат работы

Пример вывода:

Контекст A загрузил продукт. Price = 1000
Контекст B загрузил продукт. Price = 1000
Контекст A успешно сохранил изменения.
