## Практическая работа. Обработка текстовых файлов в .NET

## Цель

Студент должен уметь:

Создавать и открывать текстовые файлы.

Читать и записывать текстовую информацию.

Использовать потоковую обработку для экономии памяти.

Разрабатывать программы для анализа и обработки текстовых данных.

## Пример кода
using System;
using System.IO;

class Program
{
    static void Main()
    {
        string inputPath = "input.txt";    // путь к исходному файлу
        string outputPath = "output.txt";  // путь к файлу с результатом

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл input.txt не найден.");
            return;
        }

        // Чтение всех строк файла
        string[] lines = File.ReadAllLines(inputPath);

        // Подсчет количества строк
        int lineCount = lines.Length;

        // Запись результата в файл
        File.WriteAllText(outputPath, $"Количество строк: {lineCount}");

        Console.WriteLine($"Обработка завершена. Результат сохранен в {outputPath}");
    }
}

## Объяснение

File.Exists(inputPath) — проверка существования файла, чтобы избежать ошибок.

File.ReadAllLines(inputPath) — читает файл построчно в массив строк.

lines.Length — количество строк в файле.

File.WriteAllText(outputPath, ...) — записывает результат в новый файл.

Console.WriteLine(...) — выводим сообщение о завершении обработки.

Пример работы

## Если input.txt содержит:

Привет
Как дела?
Все хорошо


## То в output.txt будет:

Количество строк: 3