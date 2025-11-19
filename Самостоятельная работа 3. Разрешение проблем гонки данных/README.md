## Самостоятельная работа 3. Разрешение проблем гонки данных
## Пример кода
using System;
using System.Threading;

class Program
{
    static int counterLock = 0;
    static int counterMonitor = 0;
    static int counterMutex = 0;
    static int counterInterlocked = 0;

    static object lockObj = new object();
    static object monitorObj = new object();
    static Mutex mutex = new Mutex();

    // Потенциально небезопасное увеличение счётчика (гонка данных)
    static void IncreaseUnsafe()
    {
        for (int i = 0; i < 10000; i++)
        {
            counterLock++;  // используем один общий счётчик для демонстрации гонки
        }
    }

    // Безопасное увеличение с использованием lock
    static void IncreaseWithLock()
    {
        for (int i = 0; i < 10000; i++)
        {
            lock (lockObj)
            {
                counterLock++;
            }
        }
    }

    // Безопасное увеличение с использованием Monitor
    static void IncreaseWithMonitor()
    {
        for (int i = 0; i < 10000; i++)
        {
            Monitor.Enter(monitorObj);
            try
            {
                counterMonitor++;
            }
            finally
            {
                Monitor.Exit(monitorObj);
            }
        }
    }

    // Безопасное увеличение с использованием Mutex
    static void IncreaseWithMutex()
    {
        for (int i = 0; i < 10000; i++)
        {
            mutex.WaitOne();
            counterMutex++;
            mutex.ReleaseMutex();
        }
    }

    // Безопасное увеличение с использованием Interlocked
    static void IncreaseWithInterlocked()
    {
        for (int i = 0; i < 10000; i++)
        {
            Interlocked.Increment(ref counterInterlocked);
        }
    }

    static void Main()
    {
        Console.WriteLine("Демонстрация гонки данных:");

        // Гонка данных
        counterLock = 0;
        Thread t1 = new Thread(IncreaseUnsafe);
        Thread t2 = new Thread(IncreaseUnsafe);
        t1.Start(); t2.Start();
        t1.Join(); t2.Join();
        Console.WriteLine("Без синхронизации (гонка данных): " + counterLock);

        // Решение с lock
        counterLock = 0;
        Thread t3 = new Thread(IncreaseWithLock);
        Thread t4 = new Thread(IncreaseWithLock);
        t3.Start(); t4.Start();
        t3.Join(); t4.Join();
        Console.WriteLine("С lock: " + counterLock);

        // Решение с Monitor
        counterMonitor = 0;
        Thread t5 = new Thread(IncreaseWithMonitor);
        Thread t6 = new Thread(IncreaseWithMonitor);
        t5.Start(); t6.Start();
        t5.Join(); t6.Join();
        Console.WriteLine("С Monitor: " + counterMonitor);

        // Решение с Mutex
        counterMutex = 0;
        Thread t7 = new Thread(IncreaseWithMutex);
        Thread t8 = new Thread(IncreaseWithMutex);
        t7.Start(); t8.Start();
        t7.Join(); t8.Join();
        Console.WriteLine("С Mutex: " + counterMutex);

        // Решение с Interlocked
        counterInterlocked = 0;
        Thread t9 = new Thread(IncreaseWithInterlocked);
        Thread t10 = new Thread(IncreaseWithInterlocked);
        t9.Start(); t10.Start();
        t9.Join(); t10.Join();
        Console.WriteLine("С Interlocked: " + counterInterlocked);

        Console.WriteLine("Программа завершена.");
    }
}


# Самостоятельная работа №3
**Тема:** Разрешение проблем гонки данных

**Описание:**  
В данной работе демонстрируется гонка данных при увеличении общего счётчика несколькими потоками, а также методы её решения с использованием `lock`, `Monitor`, `Mutex` и `Interlocked`.

**Ход работы:**
1. Сначала показана гонка данных при небезопасном увеличении счётчика.
2. Далее реализованы безопасные варианты:
   - `lock` — простой механизм блокировки критической секции.
   - `Monitor` — более гибкий контроль блокировок.
   - `Mutex` — межпроцессная блокировка.
   - `Interlocked` — атомарные операции с переменными.

Результаты:
После запуска видно, что при небезопасном увеличении счётчика итоговое значение может отличаться от ожидаемого. Все синхронизированные методы дают корректный результат.
