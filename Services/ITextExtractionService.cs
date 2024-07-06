namespace Intelligent_Document_Processing_and_Analysis_API.Services;

public interface ITextExtractionService
{
    string ExtractText(string filePath, string fileExtension);
    // string ExtractTextFromPdf(string filePath);
    // string ExtractTextFromDoc(string filePath);
    // string ExtractTextFromTxt(string filePath);
}