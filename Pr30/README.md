## Практическая работа 30. Фоновые задачи. Hangfire

## Вариант 1

## 1. Сервис для фоновых задач

Services/EmailService.cs

namespace HangfireApp.Services
{
    public class EmailService
    {
        public void SendEmail(string to, string subject)
        {
            Console.WriteLine($"Email sent to {to} with subject: {subject}");
        }
    }
}

## 2. Контроллер с заданиями Hangfire

Controllers/TasksController.cs

using Hangfire;
using HangfireApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangfireApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly EmailService _emailService;

        public TasksController(EmailService emailService)
        {
            _emailService = emailService;
        }

        // Fire-and-forget
        [HttpPost("fire-and-forget")]
        public IActionResult FireAndForget()
        {
            BackgroundJob.Enqueue(() => _emailService.SendEmail("user@example.com", "Test Fire-and-Forget"));
            return Ok("Fire-and-forget task queued");
        }

        // Delayed
        [HttpPost("delayed")]
        public IActionResult Delayed()
        {
            BackgroundJob.Schedule(() => _emailService.SendEmail("user@example.com", "Delayed Task"), TimeSpan.FromMinutes(1));
            return Ok("Delayed task scheduled");
        }

        // Recurring
        [HttpPost("recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate("daily-email", () => _emailService.SendEmail("user@example.com", "Daily Email"), Cron.Daily);
            return Ok("Recurring task scheduled");
        }
    }
}

## 3. Настройка Hangfire в Program.cs

Program.cs

using Hangfire;
using Hangfire.MemoryStorage;
using HangfireApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Hangfire configuration
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseMemoryStorage()
);
builder.Services.AddHangfireServer();

// Добавляем сервис EmailService
builder.Services.AddSingleton<EmailService>();

builder.Services.AddControllers();

var app = builder.Build();

// Hangfire Dashboard доступно по /hangfire
app.UseHangfireDashboard();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});

app.Run();


```csharp
// Fire-and-forget задача
BackgroundJob.Enqueue(() => emailService.SendEmail("user@example.com", "Fire-and-Forget"));

// Delayed задача
BackgroundJob.Schedule(() => emailService.SendEmail("user@example.com", "Delayed Task"), TimeSpan.FromMinutes(1));

// Recurring задача
RecurringJob.AddOrUpdate("daily-email", () => emailService.SendEmail("user@example.com", "Daily Email"), Cron.Dai