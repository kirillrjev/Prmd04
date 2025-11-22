## Практическая работа 32. Работа с файлами в ASP.NET Core

## Вариант 1

## 1. Razor Page для загрузки файла

Pages/Upload.cshtml

@page
@model FileUploadApp.Pages.UploadModel
<h2>Загрузка файла</h2>

<form method="post" enctype="multipart/form-data">
    <input type="file" asp-for="File" />
    <button type="submit">Загрузить</button>
</form>

@if (!string.IsNullOrEmpty(Model.Message))
{
    <p>@Model.Message</p>
}

## 2. PageModel с обработкой загрузки

Pages/Upload.cshtml.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FileUploadApp.Pages
{
    public class UploadModel : PageModel
    {
        [BindProperty]
        public IFormFile File { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (File != null && File.Length > 0)
            {
                // Проверка типа файла (например, только изображения)
                var allowedExtensions = new[] { ".jpg", ".png", ".gif" };
                var extension = Path.GetExtension(File.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    Message = "Недопустимый тип файла!";
                    return Page();
                }

                // Ограничение размера файла (например, 5 МБ)
                if (File.Length > 5 * 1024 * 1024)
                {
                    Message = "Размер файла превышает 5 МБ!";
                    return Page();
                }

                // Создание уникального имени и сохранение
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Path.GetRandomFileName() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }

                Message = "Файл успешно загружен!";
            }
            else
            {
                Message = "Выберите файл для загрузки!";
            }

            return Page();
        }
    }
}

## 3

```csharp
[BindProperty]
public IFormFile File { get; set; }

public async Task<IActionResult> OnPostAsync()
{
    if (File != null && File.Length > 0)
    {
        var allowedExtensions = new[] { ".jpg", ".png", ".gif" };
        var extension = Path.GetExtension(File.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return Page(); // Ошибка типа файла

        if (File.Length > 5 * 1024 * 1024)
            return Page(); // Ошибка размера файла

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Path.GetRandomFileName() + extension;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await File.CopyToAsync(stream);
        }

        Message = "Файл успешно загружен!";
    }
    else
    {
        Message = "Выберите файл для загрузки!";
    }

    return Page();
}

Скриншоты