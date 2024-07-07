using DocumentFormat.OpenXml.Packaging;
using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Serilog;
using System.Text;

namespace Intelligent_Document_Processing_and_Analysis_API.Services;

public class TextExtractionService : ITextExtractionService
{
    public string ExtractText(string filePath, string fileExtension)
    {
        try
        {
            return fileExtension switch
            {
                Globals.PdfDocumentExtension => ExtractTextFromPdf(filePath),
                Globals.DocDocumentExtension => ExtractTextFromDoc(filePath),
                Globals.DocxDocumentExtension => ExtractTextFromDoc(filePath),
                Globals.TxtDocumentExtension => ExtractTextFromTxt(filePath),
                _ => throw new NotSupportedException($"File extension {fileExtension} is not supported.")
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(TextExtractionService), nameof(ExtractText));

            throw;
        }
    }

    private string ExtractTextFromPdf(string filePath)
    {
        try
        {
            using PdfReader reader = new(filePath);

            PdfDocument document = new(reader);
            StringBuilder text = new();

            for (int i = 1; i <= document.GetNumberOfPages(); i++)
                text.Append(PdfTextExtractor.GetTextFromPage(document.GetPage(i)));

            return text.ToString();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(TextExtractionService), nameof(ExtractTextFromPdf));

            throw;
        }
    }

    private string ExtractTextFromDoc(string filePath)
    {
        try
        {
            using WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false);

            // It will throw an exception if either MainDocumentPart or Body is null
            return doc.MainDocumentPart!.Document.Body!.InnerText;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(TextExtractionService), nameof(ExtractTextFromDoc));

            throw;
        }
    }

    private string ExtractTextFromTxt(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(TextExtractionService), nameof(ExtractTextFromTxt));

            throw;
        }
    }
}