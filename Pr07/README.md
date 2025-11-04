# Практическая работа №7: Наследование и виртуальные методы

## Вариант 1: Иерархия "Транспортные средства"

### Задание
Создать иерархию классов **"Транспортные средства"**:

- Базовый абстрактный класс: `Vehicle`  
- Производные классы: `Car`, `Motorcycle`, `Bicycle`, `Truck`  

Реализовать:
- Абстрактные методы: `StartEngine()`, `StopEngine()`, `CalculateFuelConsumption(double distance)`  
- Виртуальный метод: `DisplayInfo()`  
- Демонстрацию **полиморфизма** через коллекцию `List<Vehicle>`

---

## 📊 Диаграмма классов

![Диаграмма классов](images/class-diagram.png)

---

## 📘 Реализация
```csharp
// Models/Vehicle.cs
using System;

namespace InheritanceDemo.Models
{
    // Абстрактный базовый класс
    public abstract class Vehicle
    {
        public string Brand { get; protected set; }
        public string Model { get; protected set; }
        public double MaxSpeed { get; protected set; }
        public bool IsEngineRunning { get; private set; }

        protected Vehicle(string brand, string model, double maxSpeed)
        {
            Brand = brand;
            Model = model;
            MaxSpeed = maxSpeed;
        }

        // Абстрактные методы
        public abstract void StartEngine();
        public abstract void StopEngine();
        public abstract double CalculateFuelConsumption(double distance);

        // Виртуальный метод
        public virtual void DisplayInfo()
        {
            Console.WriteLine($" Vehicle: {Brand} {Model}");
            Console.WriteLine($"Max speed: {MaxSpeed} km/h");
            Console.WriteLine($"Engine running: {(IsEngineRunning ? "Yes" : "No")}");
        }

        protected void EngineState(bool state)
        {
            IsEngineRunning = state;
        }
    }
}

// Models/Car.cs
using System;

namespace InheritanceDemo.Models
{
    public class Car : Vehicle
    {
        public double FuelEfficiency { get; set; } // литров на 100 км

        public Car(string brand, string model, double maxSpeed, double fuelEfficiency)
            : base(brand, model, maxSpeed)
        {
            FuelEfficiency = fuelEfficiency;
        }

        public override void StartEngine()
        {
            EngineState(true);
            Console.WriteLine($"{Brand} {Model}: Двигатель запущен.");
        }

        public override void StopEngine()
        {
            EngineState(false);
            Console.WriteLine($"{Brand} {Model}: Двигатель остановлен.");
        }

        public override double CalculateFuelConsumption(double distance)
        {
            return (FuelEfficiency / 100) * distance;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("╔═══════════════ CAR ═══════════════╗");
            base.DisplayInfo();
            Console.WriteLine($"Fuel efficiency: {FuelEfficiency} L/100km");
            Console.WriteLine("╚═══════════════════════════════════╝\n");
        }
    }
}

// Models/Motorcycle.cs
using System;

namespace InheritanceDemo.Models
{
    public class Motorcycle : Vehicle
    {
        public double FuelEfficiency { get; set; }

        public Motorcycle(string brand, string model, double maxSpeed, double fuelEfficiency)
            : base(brand, model, maxSpeed)
        {
            FuelEfficiency = fuelEfficiency;
        }

        public override void StartEngine()
        {
            EngineState(true);
            Console.WriteLine($"{Brand} {Model}: Мотоцикл заведен.");
        }

        public override void StopEngine()
        {
            EngineState(false);
            Console.WriteLine($"{Brand} {Model}: Двигатель выключен.");
        }

        public override double CalculateFuelConsumption(double distance)
        {
            return (FuelEfficiency / 100) * distance;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("╔══════════ MOTORCYCLE ══════════╗");
            base.DisplayInfo();
            Console.WriteLine($"Fuel efficiency: {FuelEfficiency} L/100km");
            Console.WriteLine("╚════════════════════════════════╝\n");
        }
    }
}

// Models/Bicycle.cs
using System;

namespace InheritanceDemo.Models
{
    public class Bicycle : Vehicle
    {
        public bool HasGear { get; set; }

        public Bicycle(string brand, string model, double maxSpeed, bool hasGear)
            : base(brand, model, maxSpeed)
        {
            HasGear = hasGear;
        }

        public override void StartEngine()
        {
            Console.WriteLine($"{Brand} {Model}: У велосипеда нет двигателя!");
        }

        public override void StopEngine()
        {
            Console.WriteLine($"{Brand} {Model}: Нечего выключать — это велосипед!");
        }

        public override double CalculateFuelConsumption(double distance)
        {
            return 0; // Велосипед не потребляет топливо
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("╔════════════ BICYCLE ═══════════╗");
            base.DisplayInfo();
            Console.WriteLine($"Has gear: {(HasGear ? "Yes" : "No")}");
            Console.WriteLine("╚════════════════════════════════╝\n");
        }
    }
}

// Models/Truck.cs
using System;

namespace InheritanceDemo.Models
{
    public class Truck : Vehicle
    {
        public double LoadCapacity { get; set; } // тоннаж
        public double FuelEfficiency { get; set; }

        public Truck(string brand, string model, double maxSpeed, double loadCapacity, double fuelEfficiency)
            : base(brand, model, maxSpeed)
        {
            LoadCapacity = loadCapacity;
            FuelEfficiency = fuelEfficiency;
        }

        public override void StartEngine()
        {
            EngineState(true);
            Console.WriteLine($"{Brand} {Model}: Мощный дизель запущен.");
        }

        public override void StopEngine()
        {
            EngineState(false);
            Console.WriteLine($"{Brand} {Model}: Двигатель остановлен.");
        }

        public override double CalculateFuelConsumption(double distance)
        {
            return (FuelEfficiency / 100) * distance;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("╔══════════════ TRUCK ══════════════╗");
            base.DisplayInfo();
            Console.WriteLine($"Load capacity: {LoadCapacity} tons");
            Console.WriteLine($"Fuel efficiency: {FuelEfficiency} L/100km");
            Console.WriteLine("╚═══════════════════════════════════╝\n");
        }
    }
}

// Program.cs
using System;
using System.Collections.Generic;
using InheritanceDemo.Models;

namespace InheritanceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Демонстрация наследования и виртуальных методов\n");

            List<Vehicle> vehicles = new List<Vehicle>
            {
                new Car("Toyota", "Camry", 210, 7.5),
                new Motorcycle("Yamaha", "MT-07", 220, 5.2),
                new Bicycle("Giant", "Escape 3", 35, true),
                new Truck("Volvo", "FH16", 180, 20, 25)
            };

            Console.WriteLine("=== ИНФОРМАЦИЯ О ТРАНСПОРТНЫХ СРЕДСТВАХ ===\n");
            foreach (var v in vehicles)
                v.DisplayInfo();

            Console.WriteLine("=== ЗАПУСК ДВИГАТЕЛЕЙ ===");
            foreach (var v in vehicles)
                v.StartEngine();

            Console.WriteLine("\n=== РАСХОД ТОПЛИВА НА 150 КМ ===");
            foreach (var v in vehicles)
                Console.WriteLine($"{v.Brand} {v.Model}: {v.CalculateFuelConsumption(150):F2} л.");

            Console.WriteLine("\n=== ОСТАНОВКА ДВИГАТЕЛЕЙ ===");
            foreach (var v in vehicles)
                v.StopEngine();
        }
    }
}

 Демонстрация наследования и виртуальных методов

=== ИНФОРМАЦИЯ О ТРАНСПОРТНЫХ СРЕДСТВАХ ===
╔═══════════════ CAR ═══════════════╗
 Vehicle: Toyota Camry
Max speed: 210 km/h
Engine running: No
Fuel efficiency: 7.5 L/100km
╚═══════════════════════════════════╝

╔══════════ MOTORCYCLE ══════════╗
 Vehicle: Yamaha MT-07
Max speed: 220 km/h
Engine running: No
Fuel efficiency: 5.2 L/100km
╚════════════════════════════════╝

╔════════════ BICYCLE ═══════════╗
 Vehicle: Giant Escape 3
Max speed: 35 km/h
Engine running: No
Has gear: Yes
╚════════════════════════════════╝

╔══════════════ TRUCK ══════════════╗
 Vehicle: Volvo FH16
Max speed: 180 km/h
Engine running: No
Load capacity: 20 tons
Fuel efficiency: 25 L/100km
╚═══════════════════════════════════╝

=== ЗАПУСК ДВИГАТЕЛЕЙ ===
Toyota Camry: Двигатель запущен.
Yamaha MT-07: Мотоцикл заведен.
Giant Escape 3: У велосипеда нет двигателя!
Volvo FH16: Мощный дизель запущен.

=== РАСХОД ТОПЛИВА НА 150 КМ ===
Toyota Camry: 11.25 л.
Yamaha MT-07: 7.80 л.
Giant Escape 3: 0.00 л.
Volvo FH16: 37.50 л.

=== ОСТАНОВКА ДВИГАТЕЛЕЙ ===
Toyota Camry: Двигатель остановлен.
Yamaha MT-07: Двигатель выключен.
Giant Escape 3: Нечего выключать — это велосипед!
Volvo FH16: Двигатель остановлен.
