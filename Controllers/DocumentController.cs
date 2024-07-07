using Intelligent_Document_Processing_and_Analysis_API.Models.LLM;
using Intelligent_Document_Processing_and_Analysis_API.Services;
using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController(ISaveFileService saveFileService, ITextExtractionService textExtractionService, ILlmCompletionService llmCompletionService) : ControllerBase
{
    private readonly ISaveFileService _saveFileService = saveFileService ?? throw new ArgumentNullException(nameof(saveFileService));
    private readonly ITextExtractionService _textExtractionService = textExtractionService ?? throw new ArgumentNullException(nameof(textExtractionService));
    private readonly ILlmCompletionService _llmCompletionService = llmCompletionService ?? throw new ArgumentNullException(nameof(llmCompletionService));

    [HttpPost("summary")]
    public async Task<ActionResult<string>> GetDocumentSummary(IFormFile file)
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

            CompletionRequest completionRequest = new(CompletionRequestsConstants.summaryCompletionRequest.Messages, CompletionRequestsConstants.summaryCompletionRequest.Temperature);

            completionRequest.Messages.Add(new(Globals.CompletionMessageRoleUser, text));

            var summary = await _llmCompletionService.GetCompletionAsync(completionRequest).ConfigureAwait(false);

            if (summary == null || summary.Choices == null || summary.Choices.Count == 0)
                return StatusCode(500, "An error occurred while getting the completion.");

            return Ok(summary.Choices[0].Message.Content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(DocumentController), nameof(TestUploadDocument));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }

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