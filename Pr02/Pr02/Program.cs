using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите ваш балл (0-100): ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int score))
        {
            Console.WriteLine("Ошибка: Введите числовое значение.");
            return;
        }

        if (score < 0 || score > 100)
        {
            Console.WriteLine("Ошибка: Балл должен быть от 0 до 100.");
            return;
        }

        string grade;

        if (score >= 90)
        {
            grade = "A";
        }
        else if (score >= 80)
        {
            grade = "B";
        }
        else if (score >= 70)
        {
            grade = "C";
        }
        else if (score >= 60)
        {
            grade = "D";
        }
        else
        {
            grade = "F";
        }

        Console.WriteLine($"Ваша буквенная оценка: {grade}");
    }
}
