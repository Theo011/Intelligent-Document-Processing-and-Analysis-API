using Intelligent_Document_Processing_and_Analysis_API.Services;
using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController(ISaveFileService saveFileService, ITextExtractionService textExtractionService) : ControllerBase
{
    private readonly ISaveFileService _saveFileService = saveFileService ?? throw new ArgumentNullException(nameof(saveFileService));
    private readonly ITextExtractionService _textExtractionService = textExtractionService ?? throw new ArgumentNullException(nameof(textExtractionService));

    [HttpPost("testupload")]
    public async Task<ActionResult<string>> TestUploadDocument(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!Globals.allowedDocumentExtensions.Contains(fileExtension))
                return BadRequest($"File extension {fileExtension} is not allowed. Only {Globals.allowedDocumentExtensionsString} are allowed.");

            var filePath = await _saveFileService.SaveFileAsync(file, fileExtension).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(filePath))
                return StatusCode(500, "An error occurred while saving the file.");

            var text = _textExtractionService.ExtractText(filePath, fileExtension);

            if (string.IsNullOrWhiteSpace(text))
                return StatusCode(500, "An error occurred while extracting text from the file.");

            return Ok(text);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(DocumentController), nameof(TestUploadDocument));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }
}