## Практическая работа 11. Использование Task
## Пример кода
using System;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Создание и запуск задачи
        Task printNumbersTask = Task.Run(() =>
        {
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine(i);
                Task.Delay(100).Wait(); // имитация работы
            }
        });

        // Ожидание завершения задачи
        printNumbersTask.Wait();

        Console.WriteLine("Задача завершена.");
    }
}


## Пояснения:

Task.Run(() => { ... }) — создаёт и запускает асинхронную задачу.

Цикл for выводит числа от 1 до 10.

Task.Delay(100).Wait() — небольшой таймаут, чтобы имитировать время выполнения.

printNumbersTask.Wait() — программа ждёт завершения задачи перед выводом сообщения о завершении.

# Практическая работа №11
**Тема:** Использование Task  
**Вариант:** 1

**Задание:** Создать Task, который выводит числа от 1 до 10.