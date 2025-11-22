## Практическая работа 33. Логгирование. Serilog

## Вариант 1

## 1. Настройка Serilog

Program.cs

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // вывод логов в консоль
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // лог в файл с ежедневным роллингом
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

var app = builder.Build();

// Включение логирования HTTP-запросов
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();

## 2. Контроллер с логированием

Controllers/SampleController.cs

using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace SerilogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            // Логирование информационного сообщения
            Log.Information("Тестовый запрос выполнен в {Time}", DateTime.Now);

            try
            {
                // Искусственное исключение для проверки логирования ошибок
                throw new Exception("Тестовое исключение");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Произошла ошибка при выполнении запроса");
            }

            // Логирование предупреждения
            Log.Warning("Попытка доступа к потенциально небезопасному ресурсу");

            return Ok("Проверка логирования выполнена");
        }
    }
}

## 3. Конфигурация через appsettings.json (опционально)
{
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "Logs/log-.txt", "rollingInterval": "Day" } }
    ]
  }
}
## 4

```csharp
// Program.cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

// Контроллер
Log.Information("Пользователь {User} вошел в систему", "admin");
Log.Warning("Попытка доступа к запрещенному ресурсу");
Log.Error(new InvalidOperationException("Ошибка операции"), "Произошла ошибка");