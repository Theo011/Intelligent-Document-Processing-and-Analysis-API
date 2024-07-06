using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Services;

public class SaveFileService : ISaveFileService
{
    public async Task<string> SaveFileAsync(IFormFile file, string fileExtension)
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(Globals.UploadsFolderName, fileName);

            Directory.CreateDirectory(Globals.UploadsFolderName);

            using FileStream stream = new(filePath, FileMode.Create);

            await file.CopyToAsync(stream).ConfigureAwait(false);

            return filePath;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(SaveFileService), nameof(SaveFileAsync));

            throw;
        }
    }
}