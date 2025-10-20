# Практическая работа №2: Условные выражения в C#
## Вариант 1: Калькулятор оценок

### Цель работы
Изучить и применить условные операторы в языке C#.

### Задачи
- Освоить конструкции `if`, `else if`, `else`.
- Реализовать программу для оценки баллов.
- Использовать Git и Gogs для контроля версий.

### Описание задания
Программа запрашивает числовой балл от пользователя (0–100) и выводит буквенную оценку по шкале A–F.

### Инструкция по запуску
1. Открыть терминал
2. Перейти в папку `ConditionalPractice`
3. Выполнить команду `dotnet run`

### Скриншоты
- `test_1.png` — Ввод: 100 → Результат: A
- `test_1.png` — Ввод: 59 → Результат: F

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
