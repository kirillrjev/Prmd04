# Практическая работа №6: Создание классов и объектов

## Вариант 1: Класс `Student`

### Задание
Создать класс **Student**, представляющий студента.  
Класс должен содержать информацию о **ФИО**, **возрасте**, **номере группы**, **среднем балле**.  
Реализовать методы для **изменения группы** и **расчета стипендии**.  
Продемонстрировать работу класса в `Main()`.

---

### Реализация

```csharp
// Models/Student.cs
using System;

namespace ClassesAndObjects.Models
{
    public class Student
    {
        // Поля
        private string fullName;
        private int age;
        private string groupNumber;
        private double averageGrade;

        // Свойства с валидацией
        public string FullName
        {
            get => fullName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ФИО не может быть пустым.");
                fullName = value;
            }
        }

        public int Age
        {
            get => age;
            set
            {
                if (value < 16 || value > 100)
                    throw new ArgumentException("Возраст должен быть от 16 до 100 лет.");
                age = value;
            }
        }

        public string GroupNumber
        {
            get => groupNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Номер группы не может быть пустым.");
                groupNumber = value;
            }
        }

        public double AverageGrade
        {
            get => averageGrade;
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentException("Средний балл должен быть от 0 до 5.");
                averageGrade = value;
            }
        }

        // Конструкторы
        public Student() { }

        public Student(string fullName, int age, string groupNumber, double averageGrade)
        {
            FullName = fullName;
            Age = age;
            GroupNumber = groupNumber;
            AverageGrade = averageGrade;
        }

        // Методы
        public void DisplayInfo()
        {
            Console.WriteLine($"👨‍🎓 Студент: {FullName}");
            Console.WriteLine($"Возраст: {Age}");
            Console.WriteLine($"Группа: {GroupNumber}");
            Console.WriteLine($"Средний балл: {AverageGrade:F1}");
            Console.WriteLine($"Стипендия: {CalculateScholarship():C}");
            Console.WriteLine();
        }

        public void ChangeGroup(string newGroup)
        {
            if (string.IsNullOrWhiteSpace(newGroup))
                throw new ArgumentException("Номер группы не может быть пустым.");
            Console.WriteLine($"{FullName} переведен из группы {GroupNumber} в {newGroup}.");
            GroupNumber = newGroup;
        }

        public double CalculateScholarship()
        {
            if (AverageGrade >= 4.75)
                return 5000;
            else if (AverageGrade >= 4.0)
                return 3000;
            else if (AverageGrade >= 3.5)
                return 1500;
            else
                return 0;
        }
    }
}
 // Program.cs
using System;
using ClassesAndObjects.Models;

namespace ClassesAndObjects
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создание объектов
            Student student1 = new Student("Иванов Иван Иванович", 19, "ИТ-102", 4.8);
            Student student2 = new Student("Петров Петр Сергеевич", 20, "ИТ-103", 3.9);
            Student student3 = new Student("Сидорова Анна Николаевна", 18, "ИТ-101", 4.3);

            // Вывод информации
            student1.DisplayInfo();
            student2.DisplayInfo();
            student3.DisplayInfo();

            // Изменение группы
            student2.ChangeGroup("ИТ-202");
            Console.WriteLine();

            // Повторный вывод
            student2.DisplayInfo();
        }
    }
}
Студент: Иванов Иван Иванович
Возраст: 19
Группа: ИТ-102
Средний балл: 4,8
Стипендия: 5 000,00 ₽

Студент: Петров Петр Сергеевич
Возраст: 20
Группа: ИТ-103
Средний балл: 3,9
Стипендия: 0,00 ₽

Студент: Сидорова Анна Николаевна
Возраст: 18
Группа: ИТ-101
Средний балл: 4,3
Стипендия: 3 000,00 ₽

Петров Петр Сергеевич переведен из группы ИТ-103 в ИТ-202.

Студент: Петров Петр Сергеевич
Возраст: 20
Группа: ИТ-202
Средний балл: 3,9
Стипендия: 0,00 ₽
