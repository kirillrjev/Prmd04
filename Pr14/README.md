## Практическая работа 14. Работа с Word в .NET

using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

class Program
{
    static void Main()
    {
        string filepath = "Example.docx"; // имя создаваемого документа

        // Создание документа Word
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(
            filepath, 
            DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
        {
            // Добавляем основной раздел документа
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new Document();
            Body body = mainPart.Document.AppendChild(new Body());

            // Добавляем заголовок
            Paragraph heading = new Paragraph(
                new Run(
                    new Text("Заголовок документа")
                )
            );
            body.AppendChild(heading);

            // Добавляем абзац текста
            Paragraph paragraph = new Paragraph(
                new Run(
                    new Text("Это пример текста в Word-документе.")
                )
            );
            body.AppendChild(paragraph);
        }

        Console.WriteLine("Документ Example.docx создан успешно.");
    }
}
## Объяснение кода
WordprocessingDocument.Create — создаёт новый .docx файл.

MainDocumentPart — основной раздел документа, куда добавляется тело.

Body — тело документа, содержит абзацы и таблицы.

Paragraph — абзац текста.

Run — контейнер для форматирования текста в абзаце.

Text — непосредственно текст, который будет виден в документе.

using — гарантирует корректное закрытие документа после работы.

## Результат
После выполнения программы создаётся файл Example.docx с содержимым:

## mathematica
Копировать код
Заголовок документа
Это пример текста в Word-документе.