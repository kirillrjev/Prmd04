## Самостоятельная работа 4. Чтение и запись файлов большого объема

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string bigFile = "bigdata.txt";
        string copyFile = "copy_async.txt";

        // Генерация большого файла (50 МБ+)
        Console.WriteLine("Генерация файла...");
        var swGen = Stopwatch.StartNew();
        using (var writer = new StreamWriter(bigFile, false, Encoding.UTF8, bufferSize: 8192))
        {
            for (int i = 0; i < 5_000_000; i++)
            {
                writer.WriteLine($"Строка номер {i}");
            }
        }
        swGen.Stop();
        Console.WriteLine($"Файл создан за {swGen.Elapsed.TotalSeconds:F2} сек.");

        // Построчное чтение файла
        Console.WriteLine("\nЧтение файла...");
        var swRead = Stopwatch.StartNew();
        int count = 0;

        using (var reader = new StreamReader(bigFile, Encoding.UTF8, true, 8192))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
                count++;
        }

        swRead.Stop();
        Console.WriteLine($"Прочитано {count} строк за {swRead.Elapsed.TotalSeconds:F2} сек.");

        // Асинхронная запись копии файла
        Console.WriteLine("\nАсинхронная запись файла...");
        var swWrite = Stopwatch.StartNew();

        using (var reader = new StreamReader(bigFile, Encoding.UTF8, true, 8192))
        using (var writer = new StreamWriter(copyFile, false, Encoding.UTF8, 8192))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                await writer.WriteLineAsync(line);
            }
        }

        swWrite.Stop();
        Console.WriteLine($"Файл записан асинхронно за {swWrite.Elapsed.TotalSeconds:F2} сек.");
    }
}
## Объяснение кода
Генерация файла

Используется StreamWriter с буферизацией (bufferSize: 8192) для экономии обращений к диску.

Создается файл размером около 50 МБ с 5 миллионами строк.

Построчное чтение

StreamReader.ReadLineAsync() читается асинхронно, не загружая весь файл в память.

Считается количество строк и измеряется время выполнения.

Асинхронная запись

Чтение и запись выполняются построчно с await WriteLineAsync().

Используется буферизация для повышения производительности.

Измерение времени

Класс Stopwatch позволяет сравнить производительность генерации, чтения и записи файла.

## Пример вывода в консоль
Копировать код
Генерация файла...
Файл создан за 12.34 сек.

Чтение файла...
Прочитано 5000000 строк за 8.21 сек.

Асинхронная запись файла...
Файл записан асинхронно за 10.56 сек.