## Практическая работа 21. CRUD-операции с Entity Framework

## Вариант 1

## Модель Product
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

## Контекст данных
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=.;Database=CrudProductsDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}

## CRUD-операции
using var db = new AppDbContext();

// CREATE
db.Products.Add(new Product { Name = "Laptop", Price = 59999 });
db.SaveChanges();

// READ
var products = db.Products.ToList();

// UPDATE
var first = products.First();
first.Price = 54999;
db.Products.Update(first);
db.SaveChanges();

// DELETE
db.Products.Remove(first);
db.SaveChanges();

🗄 Создание базы (миграции)
dotnet ef migrations add InitialCreate
dotnet ef database update