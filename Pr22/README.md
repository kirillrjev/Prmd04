## Практическая работа 22. Миграции и управление схемой базы данных в EF Core

## Вариант 1

## 1. Модель Product (первая версия — без Description)

Project/Models/Product.cs

namespace Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

## 2. DbContext

Project/Data/AppDbContext.cs

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
                "Server=.;Database=MigrationsDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

## 3. Создание первой миграции

В терминале, внутри папки Project:

dotnet ef migrations add InitialCreate
dotnet ef database update


После этого создастся таблица Products с колонками:
Id, Name, Price.

## 4. Изменение модели — добавляем Description

Product.cs (обновлённая версия)

namespace Project.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; } // новое поле
    }
}