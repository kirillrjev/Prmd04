# Практическая работа №4: Классы и объекты
## Вариант 1: Класс "Книга"

### Цель работы
Освоить создание классов, объектов, методов, свойств и конструкторов в C#.

### Задачи
- Создать класс `Book` с полями, свойствами и методами.
- Использовать несколько конструкторов.
- Проверять корректность данных через свойства.
- Реализовать метод, определяющий, является ли книга старой.

### Описание задания
Создайте класс `Book` с полями: название, автор, год издания, количество страниц.  
Реализуйте методы:
- `DisplayInfo()` — выводит информацию о книге.
- `IsOld()` — возвращает `true`, если книге больше 50 лет.  
Добавьте свойства с проверкой корректности значений и конструкторы: по умолчанию и с параметрами.

### Сборка и запуск
1. Откройте терминал.
2. Перейдите в папку `Pr04`.
3. Выполните команду:

using System;

namespace BookPractice
{
    public class Book
    {
        private string title;
        private string author;
        private int year;
        private int pages;

        public string Title
        {
            get => title;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    title = value;
            }
        }

        public string Author
        {
            get => author;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    author = value;
            }
        }

        public int Year
        {
            get => year;
            set
            {
                int currentYear = DateTime.Now.Year;
                if (value > 1400 && value <= currentYear)
                    year = value;
            }
        }

        public int Pages
        {
            get => pages;
            set
            {
                if (value > 0)
                    pages = value;
            }
        }
        public Book()
        {
            title = "Неизвестно";
            author = "Неизвестен";
            year = DateTime.Now.Year;
            pages = 100;
        }

        public Book(string title, string author, int year, int pages)
        {
            Title = title;
            Author = author;
            Year = year;
            Pages = pages;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Книга: {Title}");
            Console.WriteLine($"Автор: {Author}");
            Console.WriteLine($"Год издания: {Year}");
            Console.WriteLine($"Страниц: {Pages}");
            Console.WriteLine($"Старая книга: {(IsOld() ? "Да" : "Нет")}");
        }

        public bool IsOld()
        {
            return DateTime.Now.Year - year > 50;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Book book1 = new Book("Преступление и наказание", "Фёдор Достоевский", 1866, 671);
            book1.DisplayInfo();

            Console.WriteLine();

            Book book2 = new Book();
            book2.Title = "C# для начинающих";
            book2.Author = "Ржевский Кирилл";
            book2.Year = 2025;
            book2.Pages = 350;
            book2.DisplayInfo();
        }
    }
}
