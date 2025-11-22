## Практическая работа 28. Разработка приложения Blazor

## Вариант 1

## 1. Модель данных

Models/Product.cs

namespace MyBlazorApp.Models
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

## 2. Компонент Blazor

Pages/ProductList.razor

@page "/products"
@using MyBlazorApp.Models

<h3>Список продуктов</h3>

<ul>
    @foreach(var p in products)
    {
        <li>@p.Name - @p.Price ₽</li>
    }
</ul>

<h3>Добавить продукт</h3>

<input placeholder="Название" @bind="newProduct.Name" />
<input type="number" placeholder="Цена" @bind="newProduct.Price" />

<button @onclick="AddProduct">Добавить</button>

@code {
    private List<Product> products = new List<Product>();
    private Product newProduct = new Product();

    private void AddProduct()
    {
        if (!string.IsNullOrWhiteSpace(newProduct.Name) && newProduct.Price > 0)
        {
            products.Add(new Product { Name = newProduct.Name, Price = newProduct.Price });
            newProduct = new Product();
        }
    }
}

## 3. Настройка маршрутизации

В App.razor:

<Router AppAssembly="@typeof(App).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <h3>Страница не найдена</h3>
    </NotFound>
</Router>


Добавьте ссылку на компонент в NavMenu.razor:

<NavLink class="nav-link" href="products">
    Продукты
</NavLink>


### Модель данных

```csharp
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

Компонент ProductList.razor
<input placeholder="Название" @bind="newProduct.Name" />
<input type="number" placeholder="Цена" @bind="newProduct.Price" />
<button @onclick="AddProduct">Добавить</button>

<ul>
    @foreach(var p in products)
    {
        <li>@p.Name - @p.Price ₽</li>
    }
</ul>

@code {
    private List<Product> products = new List<Product>();
    private Product newProduct = new Product();

    private void AddProduct()
    {
        if (!string.IsNullOrWhiteSpace(newProduct.Name) && newProduct.Price > 0)
        {
            products.Add(new Product { Name = newProduct.Name, Price = newProduct.Price });
            newProduct = new Product();
        }
    }
}