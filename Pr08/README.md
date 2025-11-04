# Практическая работа №8: Интерфейсы и абстрактные классы

## Вариант 1: Система оплаты

###  Задание
Создать систему **оплаты**, реализующую интерфейсы:

- `IPayable` — возможность совершать оплату  
- `IRefundable` — возможность делать возврат  
- `IReceiptable` — возможность выдавать чек  

Абстрактный класс — `Payment` (базовый для всех видов платежей)

Конкретные классы:
- `CreditCardPayment`
- `PayPalPayment`
- `BankTransferPayment`

Необходимо:
- Использовать множественную реализацию интерфейсов  
- Реализовать методы по умолчанию в интерфейсах (C# 8.0+)  
- Продемонстрировать полиморфизм через базовый класс и интерфейсы  

---

## Реализация

### Интерфейсы
```csharp
// Models/Interfaces/IPayable.cs
using System;

namespace InterfacesDemo.Models.Interfaces
{
    public interface IPayable
    {
        bool ProcessPayment(decimal amount);
        string PaymentMethod { get; }

        // Метод по умолчанию
        void ShowPaymentInfo()
        {
            Console.WriteLine($"Processing payment via {PaymentMethod}...");
        }
    }
}
 
// Models/Interfaces/IRefundable.cs
using System;

namespace InterfacesDemo.Models.Interfaces
{
    public interface IRefundable
    {
        bool Refund(decimal amount);
        DateTime LastRefundDate { get; set; }

        void DefaultRefundPolicy()
        {
            Console.WriteLine("Refund allowed within 30 days.");
        }
    }
}

// Models/Interfaces/IReceiptable.cs
using System;

namespace InterfacesDemo.Models.Interfaces
{
    public interface IReceiptable
    {
        void GenerateReceipt(decimal amount, string recipient);
        string ReceiptFormat { get; set; }
    }
}

// Models/Abstract/Payment.cs
using System;

namespace InterfacesDemo.Models.Abstract
{
    public abstract class Payment
    {
        public string TransactionId { get; protected set; }
        public string PayerName { get; protected set; }
        public decimal Amount { get; protected set; }
        public DateTime PaymentDate { get; protected set; }

        protected Payment(string payerName, decimal amount)
        {
            TransactionId = Guid.NewGuid().ToString();
            PayerName = payerName;
            Amount = amount;
            PaymentDate = DateTime.Now;
        }

        public abstract void Process();
        public virtual void DisplayInfo()
        {
            Console.WriteLine($" Transaction: {TransactionId}");
            Console.WriteLine($"Payer: {PayerName}");
            Console.WriteLine($"Amount: {Amount:C}");
            Console.WriteLine($"Date: {PaymentDate:yyyy-MM-dd HH:mm}");
        }
    }
}

// Models/Concrete/CreditCardPayment.cs
using System;
using InterfacesDemo.Models.Abstract;
using InterfacesDemo.Models.Interfaces;

namespace InterfacesDemo.Models.Concrete
{
    public class CreditCardPayment : Payment, IPayable, IRefundable, IReceiptable
    {
        public string CardNumber { get; private set; }
        public string PaymentMethod => "Credit Card";
        public DateTime LastRefundDate { get; set; }
        public string ReceiptFormat { get; set; } = "PDF";

        public CreditCardPayment(string payerName, decimal amount, string cardNumber)
            : base(payerName, amount)
        {
            CardNumber = cardNumber;
        }

        public override void Process()
        {
            Console.WriteLine($" Processing credit card payment for {PayerName}");
            ProcessPayment(Amount);
            GenerateReceipt(Amount, PayerName);
        }

        public bool ProcessPayment(decimal amount)
        {
            ShowPaymentInfo();
            Console.WriteLine($"Charged {amount:C} to card ****{CardNumber[^4..]}");
            return true;
        }

        public bool Refund(decimal amount)
        {
            DefaultRefundPolicy();
            Console.WriteLine($" Refunded {amount:C} to credit card ****{CardNumber[^4..]}");
            LastRefundDate = DateTime.Now;
            return true;
        }

        public void GenerateReceipt(decimal amount, string recipient)
        {
            Console.WriteLine($" Receipt generated for {recipient} in {ReceiptFormat}");
        }
    }
}

// Models/Concrete/PayPalPayment.cs
using System;
using InterfacesDemo.Models.Abstract;
using InterfacesDemo.Models.Interfaces;

namespace InterfacesDemo.Models.Concrete
{
    public class PayPalPayment : Payment, IPayable, IReceiptable
    {
        public string Email { get; private set; }
        public string PaymentMethod => "PayPal";
        public string ReceiptFormat { get; set; } = "Email";

        public PayPalPayment(string payerName, decimal amount, string email)
            : base(payerName, amount)
        {
            Email = email;
        }

        public override void Process()
        {
            Console.WriteLine($"💰 Processing PayPal payment from {Email}");
            ProcessPayment(Amount);
            GenerateReceipt(Amount, Email);
        }

        public bool ProcessPayment(decimal amount)
        {
            ShowPaymentInfo();
            Console.WriteLine($"Transferred {amount:C} from PayPal account {Email}");
            return true;
        }

        public void GenerateReceipt(decimal amount, string recipient)
        {
            Console.WriteLine($" Email receipt sent to {recipient}");
        }
    }
}

// Models/Concrete/BankTransferPayment.cs
using System;
using InterfacesDemo.Models.Abstract;
using InterfacesDemo.Models.Interfaces;

namespace InterfacesDemo.Models.Concrete
{
    public class BankTransferPayment : Payment, IPayable, IRefundable
    {
        public string BankAccount { get; private set; }
        public string PaymentMethod => "Bank Transfer";
        public DateTime LastRefundDate { get; set; }

        public BankTransferPayment(string payerName, decimal amount, string bankAccount)
            : base(payerName, amount)
        {
            BankAccount = bankAccount;
        }

        public override void Process()
        {
            Console.WriteLine($" Processing bank transfer from {PayerName}");
            ProcessPayment(Amount);
        }

        public bool ProcessPayment(decimal amount)
        {
            ShowPaymentInfo();
            Console.WriteLine($"Transferred {amount:C} from bank account {BankAccount}");
            return true;
        }

        public bool Refund(decimal amount)
        {
            DefaultRefundPolicy();
            Console.WriteLine($"Refunded {amount:C} to bank account {BankAccount}");
            LastRefundDate = DateTime.Now;
            return true;
        }
    }
}

// Program.cs
using System;
using System.Collections.Generic;
using InterfacesDemo.Models.Abstract;
using InterfacesDemo.Models.Concrete;
using InterfacesDemo.Models.Interfaces;

namespace InterfacesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Демонстрация интерфейсов и абстрактных классов (Система оплаты)\n");

            List<Payment> payments = new List<Payment>
            {
                new CreditCardPayment("Alice Johnson", 1500.50m, "1234567890123456"),
                new PayPalPayment("Bob Smith", 999.99m, "bob@paypal.com"),
                new BankTransferPayment("Charlie Brown", 2500m, "BY00BANK123456789")
            };

            Console.WriteLine("=== ПРОЦЕССИНГ ПЛАТЕЖЕЙ ===");
            foreach (var p in payments)
            {
                p.Process();
                Console.WriteLine();
            }

            Console.WriteLine("=== ВОЗВРАТ СРЕДСТВ ===");
            foreach (var r in payments.OfType<IRefundable>())
            {
                r.Refund(200m);
                Console.WriteLine();
            }

            Console.WriteLine("=== ВЫДАЧА ЧЕКОВ ===");
            foreach (var rec in payments.OfType<IReceiptable>())
            {
                rec.GenerateReceipt(recipient: "finance@company.com", amount: 300m);
                Console.WriteLine();
            }

            Console.WriteLine("=== СТАТИСТИКА ===");
            Console.WriteLine($"Total payments: {payments.Count}");
            Console.WriteLine($"Refundable: {payments.OfType<IRefundable>().Count()}");
            Console.WriteLine($"Receiptable: {payments.OfType<IReceiptable>().Count()}");
        }
    }
}

 Демонстрация интерфейсов и абстрактных классов (Система оплаты)

=== ПРОЦЕССИНГ ПЛАТЕЖЕЙ ===
 Processing credit card payment for Alice Johnson
Processing payment via Credit Card...
Charged ₽1,500.50 to card ****3456
 Receipt generated for Alice Johnson in PDF

 Processing PayPal payment from bob@paypal.com
Processing payment via PayPal...
Transferred ₽999.99 from PayPal account bob@paypal.com
 Email receipt sent to bob@paypal.com

 Processing bank transfer from Charlie Brown
Processing payment via Bank Transfer...
Transferred ₽2,500.00 from bank account BY00BANK123456789

=== ВОЗВРАТ СРЕДСТВ ===
Refund allowed within 30 days.
 Refunded ₽200.00 to credit card ****3456

Refund allowed within 30 days.
Refunded ₽200.00 to bank account BY00BANK123456789

=== ВЫДАЧА ЧЕКОВ ===
 Receipt generated for finance@company.com in PDF

 Email receipt sent to finance@company.com

=== СТАТИСТИКА ===
Total payments: 3
Refundable: 2
Receiptable: 2

