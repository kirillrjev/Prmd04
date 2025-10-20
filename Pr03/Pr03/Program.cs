using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите число для вывода таблицы умножения (1-10): ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int number))
        {
            Console.WriteLine("Ошибка: введено не число.");
            return;
        }

        if (number < 1 || number > 10)
        {
            Console.WriteLine("Ошибка: число должно быть от 1 до 10.");
            return;
        }

        Console.WriteLine($"\nТаблица умножения для {number}:");

        // Цикл for от 1 до 10
        for (int i = 1; i <= 10; i++)
        {
            int result = number * i;
            Console.WriteLine($"{number} × {i} = {result}");
        }
    }
}
