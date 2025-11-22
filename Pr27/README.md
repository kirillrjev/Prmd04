## Практическая работа 27. Разработка приложения MVC

## Вариант 1

## 1. Модель данных

Models/Product.cs

namespace MyMvcApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

## 2. Контроллер

Controllers/ProductsController.cs

using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyMvcApp.Controllers
{
    public class ProductsController : Controller
    {
        private static List<Product> products = new List<Product>();

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = products.Count + 1;
                products.Add(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
            }
            return RedirectToAction("Index");
        }
    }
}

## 3. Представления

Views/Products/Index.cshtml

@model List<MyMvcApp.Models.Product>
@{
    ViewData["Title"] = "Список продуктов";
}

<h2>Список продуктов</h2>
<table>
    <tr>
        <th>Id</th>
        <th>Название</th>
        <th>Цена</th>
        <th>Действия</th>
    </tr>
    @foreach(var item in Model)
    {
        <tr>
            <td>@item.Id</td>
            <td>@item.Name</td>
            <td>@item.Price</td>
            <td>
                <a asp-action="Delete" asp-route-id="@item.Id">Удалить</a>
            </td>
        </tr>
    }
</table>
<a asp-action="Create">Добавить продукт</a>


Views/Products/Create.cshtml

@model MyMvcApp.Models.Product
@{
    ViewData["Title"] = "Добавление продукта";
}

<h2>Добавление продукта</h2>

<form asp-action="Create" method="post">
    <div>
        <label>Название:</label>
        <input asp-for="Name" />
    </div>
    <div>
        <label>Цена:</label>
        <input asp-for="Price" />
    </div>
    <button type="submit">Добавить</button>
</form>

## 4. Настройка маршрутизации

В Program.cs для ASP.NET Core 6+:

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
