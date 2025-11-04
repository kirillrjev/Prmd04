# Практическая работа 5 — Основы LINQ to Objects

## Тема
Работа с LINQ to Objects для обработки коллекций данных в C#.

## Цель
Научиться применять LINQ для фильтрации, сортировки, группировки, проекции и агрегации данных.

## Структура проекта
- `Employee.cs` — класс сущности "Сотрудник".
- `Program.cs` — основной код с тестовыми данными и запросами LINQ.
- `/images` — примеры вывода программы (необязательно).

## Как запустить
1. Убедитесь, что установлен .NET SDK 6 или выше.
2. Клонируйте репозиторий.
3. В терминале выполните:
   ```bash
   cd LinqPractice
   dotnet run


---

## 📄 LinqPractice.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>

## Employee.cs
namespace LinqPractice;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Age { get; set; }
    public string Department { get; set; } = "";
    public decimal Salary { get; set; }
}
## Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using LinqPractice;

namespace LinqPractice
{
    public class Program
    {
        public static void Main()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "Ivan", LastName = "Ivanov", Age = 30, Department = "IT", Salary = 50000 },
                new Employee { Id = 2, FirstName = "Petr", LastName = "Petrov", Age = 25, Department = "HR", Salary = 40000 },
                new Employee { Id = 3, FirstName = "Maria", LastName = "Sidorova", Age = 35, Department = "Finance", Salary = 60000 },
                new Employee { Id = 4, FirstName = "Anna", LastName = "Kuznetsova", Age = 28, Department = "IT", Salary = 55000 },
                new Employee { Id = 5, FirstName = "Alexey", LastName = "Smirnov", Age = 40, Department = "Finance", Salary = 70000 },
                new Employee { Id = 6, FirstName = "Nikolay", LastName = "Popov", Age = 29, Department = "HR", Salary = 42000 },
                new Employee { Id = 7, FirstName = "Elena", LastName = "Voronina", Age = 33, Department = "IT", Salary = 65000 },
                new Employee { Id = 8, FirstName = "Sergey", LastName = "Volkov", Age = 38, Department = "Sales", Salary = 48000 },
                new Employee { Id = 9, FirstName = "Olga", LastName = "Morozova", Age = 31, Department = "Finance", Salary = 59000 },
                new Employee { Id = 10, FirstName = "Vladimir", LastName = "Stepanov", Age = 27, Department = "IT", Salary = 52000 }
            };

            // =============================
            // Вариант №1:
            // Вывести всех сотрудников IT-отдела, отсортированных по убыванию зарплаты.
            // =============================

            Console.WriteLine("=== Вариант 1: Сотрудники IT-отдела (по убыванию зарплаты) ===\n");

            // --- Способ 1: Синтаксис методов расширения ---
            var methodSyntax = employees
                .Where(e => e.Department == "IT")
                .OrderByDescending(e => e.Salary)
                .ToList();

            Console.WriteLine("Синтаксис методов расширения:");
            foreach (var emp in methodSyntax)
            {
                Console.WriteLine($"{emp.LastName} {emp.FirstName}, Отдел: {emp.Department}, Зарплата: {emp.Salary}");
            }

            Console.WriteLine();

            // --- Способ 2: Синтаксис запросов ---
            var querySyntax =
                from e in employees
                where e.Department == "IT"
                orderby e.Salary descending
                select e;

            Console.WriteLine("Синтаксис запросов:");
            foreach (var emp in querySyntax)
            {
                Console.WriteLine($"{emp.LastName} {emp.FirstName}, Отдел: {emp.Department}, Зарплата: {emp.Salary}");
            }

            Console.WriteLine();
        }
    }
}
## Контрольные вопросы — краткие ответы

LINQ to Objects работает с коллекциями в памяти (List, Array и т.п.),
а LINQ to SQL / Entities — с базами данных, генерируя SQL-запросы.

Отложенное выполнение (deferred execution) — запрос выполняется только при перечислении (например, в foreach).
Немедленное (immediate) — при вызове методов ToList(), Count(), Sum() и т.д.

First() — выбрасывает исключение, если нет элементов.
FirstOrDefault() — возвращает null (для ссылочных типов) или default (для значимых).

Select проецирует данные (создаёт новые объекты/типы).
Анонимные типы используются, когда не нужен отдельный класс для результата:

var names = employees.Select(e => new { e.FirstName, e.LastName });


GroupBy группирует по ключу, возвращая коллекцию групп:

var groups = employees.GroupBy(e => e.Department);


let — вводит промежуточную переменную внутри запроса;
into — продолжает запрос после group или join.

Лямбда-выражения — компактный способ описания анонимных функций (например, e => e.Age > 30).

Join объединяет элементы из разных коллекций по ключу:

var joined = from e in employees
             join d in departments on e.DepartmentId equals d.Id
             select new { e.FirstName, d.Name };