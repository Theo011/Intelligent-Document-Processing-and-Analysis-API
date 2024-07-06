namespace Intelligent_Document_Processing_and_Analysis_API.Services;

public interface ISaveFileService
{
    Task<string> SaveFileAsync(IFormFile file, string fileExtension);
}