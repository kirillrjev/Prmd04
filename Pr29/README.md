## Практическая работа 29. Работа с JWT токенами, ролями и разрешениями

## Вариант 1

## 1. Модель пользователя

Models/User.cs

namespace JwtAuthApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // Для учебного примера хранение в чистом виде
        public string Role { get; set; }
    }
}

## 2. Сервис генерации JWT

Services/JwtService.cs

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JwtAuthApp.Models;

namespace JwtAuthApp.Services
{
    public class JwtService
    {
        private readonly string _secret;
        public JwtService(string secret) => _secret = secret;

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "JwtAuthApp",
                audience: "JwtAuthApp",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

## 3. Контроллер аутентификации

Controllers/AuthController.cs

using JwtAuthApp.Models;
using JwtAuthApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private static List<User> users = new List<User>();

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            users.Add(user);
            return Ok("User registered");
        }

        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            var user = users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
            if (user == null) return Unauthorized();

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}

## 4. Защищенный контроллер

Controllers/ProductsController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new[] { "Product1", "Product2" });
    }
}

## 5. Настройка Program.cs

Program.cs

using JwtAuthApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

## Добавляем сервис JWT
builder.Services.AddSingleton(new JwtService("superSecretKey@345"));

// Настройка JWT аутентификации
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "JwtAuthApp",
        ValidAudience = "JwtAuthApp",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
    };
});

builder.Services.AddControllers();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();