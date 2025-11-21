## Практическая работа 17. Сравнение производительности последовательного, параллельного и асинхронного кода на примере обработки данных

## Вариант 1



using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var data = Enumerable.Range(1, 100).ToList();
        Stopwatch sw = new Stopwatch();

        // Последовательная обработка
        sw.Start();
        foreach (var item in data)
        {
            int square = Process(item);
            //Console.WriteLine($"Последовательно: {item}^2 = {square}");
        }
        sw.Stop();
        Console.WriteLine($"Последовательно: {sw.ElapsedMilliseconds} ms");

        // Параллельная обработка
        sw.Restart();
        Parallel.ForEach(data, item =>
        {
            int square = Process(item);
            //Console.WriteLine($"Параллельно: {item}^2 = {square}");
        });
        sw.Stop();
        Console.WriteLine($"Параллельно: {sw.ElapsedMilliseconds} ms");

        // Асинхронная обработка
        sw.Restart();
        Task.Run(async () =>
        {
            foreach (var item in data)
            {
                int square = await ProcessAsync(item);
                //Console.WriteLine($"Асинхронно: {item}^2 = {square}");
            }
        }).GetAwaiter().GetResult();
        sw.Stop();
        Console.WriteLine($"Асинхронно: {sw.ElapsedMilliseconds} ms");
    }

    static int Process(int value)
    {
        Thread.Sleep(10); // имитация CPU-bound работы
        return value * value;
    }

    static async Task<int> ProcessAsync(int value)
    {
        await Task.Delay(10); // имитация асинхронной работы
        return value * value;
    }
}
## Объяснение кода
Последовательная обработка
Код выполняется по одному элементу за раз. Простая реализация, но медленнее для больших массивов.

Параллельная обработка
Используется Parallel.ForEach, позволяя одновременно обрабатывать несколько элементов на разных ядрах процессора. Эффективно для CPU-bound операций.

Асинхронная обработка
Используется async/await с Task.Delay, чтобы не блокировать основной поток. Эффективно для I/O-bound операций, но не ускоряет CPU-bound задачи.

Stopwatch
Измеряет время выполнения каждого подхода для сравнения производительности.

## Результат
Консоль выведет время выполнения для каждого подхода, например:

makefile
Копировать код
Последовательно: 1023 ms
Параллельно: 315 ms
Асинхронно: 1025 ms