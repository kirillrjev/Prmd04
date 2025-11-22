## Практическая работа 26. Разработка приложения Razor Page

## Вариант 1

## 1. Модель данных

Models/Product.cs

namespace MyRazorApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

## 2. Страница Razor с формой

Pages/FormPage.cshtml

@page
@model MyRazorApp.Pages.FormPageModel
@{
    ViewData["Title"] = "Добавление продукта";
}

<h2>Добавление продукта</h2>

<form method="post">
    <label>Название:</label>
    <input type="text" asp-for="Product.Name" />
    <br/>
    <label>Цена:</label>
    <input type="number" asp-for="Product.Price" step="0.01" />
    <br/>
    <button type="submit">Добавить</button>
</form>

@if (Model.Submitted)
{
    <p>Продукт добавлен: <strong>@Model.Product.Name</strong>, цена: <strong>@Model.Product.Price</strong></p>
}

## 3. PageModel с обработкой GET/POST

Pages/FormPage.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;

namespace MyRazorApp.Pages
{
    public class FormPageModel : PageModel
    {
        [BindProperty]
        public Product Product { get; set; }

        public bool Submitted { get; set; } = false;

        public void OnGet()
        {
            // Можно инициализировать модель при GET-запросе, если нужно
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                Submitted = true;
                // Здесь можно добавить сохранение в базу через EF Core
            }
        }
    }
}

## 4. Главная страница (Index.cshtml)

Pages/Index.cshtml

@page
@model MyRazorApp.Pages.IndexModel
@{
    ViewData["Title"] = "Главная";
}

<h1>Добро пожаловать в MyRazorApp!</h1>
<p>
    <a asp-page="/FormPage">Перейти к добавлению продукта</a>
</p>


Pages/Index.cshtml.cs

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

### Модель данных

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

Razor Page (FormPage.cshtml)
<form method="post">
    <label>Название:</label>
    <input type="text" asp-for="Product.Name" />
    <label>Цена:</label>
    <input type="number" asp-for="Product.Price" />
    <button type="submit">Добавить</button>
</form>

@if (Model.Submitted)
{
    <p>Продукт добавлен: @Model.Product.Name, цена: @Model.Product.Price</p>
}

PageModel (FormPage.cshtml.cs)
[BindProperty]
public Product Product { get; set; }

public bool Submitted { get; set; } = false;

public void OnPost()
{
    if (ModelState.IsValid)
    {
        Submitted = true;
    }
}