## Практическая работа 10. Синхронизация потоков (lock, Monitor, Mutex)
Цель работы

    Научиться синхронизировать доступ нескольких потоков к общим ресурсам.
    Изучить три способа синхронизации: lock, Monitor, Mutex.
    Научиться предотвращать гонки данных (race conditions) в многопоточных приложениях.

## Пример с lock
using System;
using System.Threading;

class Program
{
    static int counter = 0;           // общий ресурс
    static object lockObj = new object(); // объект для блокировки

    static void IncreaseCounter()
    {
        for (int i = 0; i < 10; i++)
        {
            lock (lockObj) // начало критической секции
            {
                counter++;
                Console.WriteLine($"{Thread.CurrentThread.Name} увеличил счётчик: {counter}");
            } // конец критической секции

            Thread.Sleep(100); // имитация работы потока
        }
    }

    static void Main()
    {
        Thread t1 = new Thread(IncreaseCounter) { Name = "Поток 1" };
        Thread t2 = new Thread(IncreaseCounter) { Name = "Поток 2" };

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"Итоговый счётчик: {counter}");
    }
}


## Пояснения:

lock (lockObj) — критическая секция, доступ к которой может быть только у одного потока.

Thread.Sleep(100) — делает видимым, что потоки работают одновременно.

Итоговый счётчик после завершения всех потоков должен быть 20 (2 потока × 10 итераций).

## Ход работы по заданию

Создай новый проект Console App (.NET).

Скопируй код выше.

Запусти программу, сделай скриншот консоли и помести его в папку images.

Создай readme.md с указанием:

Номер работы: 10

Вариант: 1 (lock)

Текст задания.