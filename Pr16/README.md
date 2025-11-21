## Практическая работа 16. Работа с PDF в .NET
## Вариант 1

using System;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

class Program
{
    static void Main()
    {
        string filePath = "Example.pdf";

        // Создаем PDF-документ
        using (PdfWriter writer = new PdfWriter(filePath))
        using (PdfDocument pdf = new PdfDocument(writer))
        using (Document document = new Document(pdf))
        {
            // Заголовок документа
            Paragraph heading = new Paragraph("Заголовок PDF-документа")
                .SetFontSize(18)
                .SetBold();
            document.Add(heading);

            // Абзац текста
            Paragraph paragraph = new Paragraph("Пример текста в PDF-документе.");
            document.Add(paragraph);
        }

        Console.WriteLine("PDF-документ Example.pdf создан успешно.");
    }
}
## Объяснение кода
PdfWriter — объект для записи PDF в файл.

PdfDocument — основной документ PDF.

Document — тело документа, к которому добавляются элементы (текст, таблицы, изображения).

Paragraph — абзац текста; можно задавать размер шрифта, стиль, выравнивание.

using — гарантирует закрытие и сохранение PDF после завершения блока.

## Результат
Создан PDF-файл Example.pdf с:

Заголовком: "Заголовок PDF-документа"

Абзацем текста: "Пример текста в PDF-документе."