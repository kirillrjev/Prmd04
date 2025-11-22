## Практическая работа 19. Паттерн Репозиторий

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

## ФАЙЛ 2 — Data/AppDbContext.cs
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
                "Server=.;Database=RepositoryDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

## ФАЙЛ 3 — Repositories/IRepository.cs
using System.Collections.Generic;

namespace Project.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

## ФАЙЛ 4 — Repositories/Repository.cs
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id);

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}

## ФАЙЛ 5 — Program.cs

Полный пример CRUD через репозиторий.

﻿using Project.Data;
using Project.Models;
using Project.Repositories;

using var context = new AppDbContext();
IRepository<Product> productRepo = new Repository<Product>(context);

// --- CREATE ---
var product = new Product
{
    Name = "Smartphone",
    Price = 19999
};
productRepo.Add(product);
Console.WriteLine("Product added.");


// --- READ ---
Console.WriteLine("\nProducts in database:");
foreach (var p in productRepo.GetAll())
{
    Console.WriteLine($"{p.Id} | {p.Name} | {p.Price}");
}


// --- UPDATE ---
product.Price = 17999;
productRepo.Update(product);
Console.WriteLine("\nProduct updated.");


// --- DELETE ---
productRepo.Delete(product);
Console.WriteLine("\nProduct deleted.");