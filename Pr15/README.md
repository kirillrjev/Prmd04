## Практическая работа 15. Работа с Excel в .NET
## Вариант 1

using System;
using System.IO;
using OfficeOpenXml;

class Program
{
    static void Main()
    {
        string filePath = "Example.xlsx";

        // Устанавливаем контекст лицензии EPPlus
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (ExcelPackage package = new ExcelPackage())
        {
            // Создаем лист Excel
            var worksheet = package.Workbook.Worksheets.Add("Лист1");

            // Добавляем заголовки
            worksheet.Cells[1, 1].Value = "Имя";
            worksheet.Cells[1, 2].Value = "Возраст";

            // Добавляем данные
            worksheet.Cells[2, 1].Value = "Иван";
            worksheet.Cells[2, 2].Value = 25;
            worksheet.Cells[3, 1].Value = "Мария";
            worksheet.Cells[3, 2].Value = 30;
            worksheet.Cells[4, 1].Value = "Алексей";
            worksheet.Cells[4, 2].Value = 28;

            // Сохраняем файл
            package.SaveAs(new FileInfo(filePath));
        }

        Console.WriteLine("Excel-файл Example.xlsx создан успешно.");
    }
}
## Объяснение кода
ExcelPackage.LicenseContext — задаёт лицензионный контекст для EPPlus (для небизнес-использования используем NonCommercial).

ExcelPackage — основной объект для работы с Excel-файлом.

Worksheets.Add("Лист1") — создаём новый лист с именем "Лист1".

Cells[row, col].Value — записываем значения в ячейки Excel.

package.SaveAs(new FileInfo(filePath)) — сохраняем Excel-файл на диск.

using — гарантирует корректное освобождение ресурсов после работы с файлом.

## Результат
Создан файл Example.xlsx с таблицей:

Имя	Возраст
Иван	25
Мария	30
Алексей	28