## Практическая работа 31. Валидация. FluentValidation

## Вариант 1

## 1. Модель пользователя

Models/User.cs

namespace FluentValidationApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

## 2. Валидатор модели

Validators/UserValidator.cs

using FluentValidation;
using FluentValidationApp.Models;

namespace FluentValidationApp.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Имя пользователя не должно быть пустым");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email не должен быть пустым")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(u => u.Password)
                .MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов");
        }
    }
}

## 3. Контроллер API

Controllers/UsersController.cs

using FluentValidationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok("Пользователь успешно зарегистрирован");
        }
    }
}

## 4. Настройка FluentValidation в Program.cs

Program.cs

using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры с FluentValidation
builder.Services.AddControllers()
       .AddFluentValidation(fv => 
           fv.RegisterValidatorsFromAssemblyContaining<FluentValidationApp.Validators.UserValidator>());

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


public class User
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("Имя пользователя не должно быть пустым");
        RuleFor(u => u.Email).NotEmpty().EmailAddress().WithMessage("Неверный формат email");
        RuleFor(u => u.Password).MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов");
    }
}

[HttpPost("register")]
public IActionResult Register(User user)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);
    return Ok("Пользователь успешно зарегистрирован");
}