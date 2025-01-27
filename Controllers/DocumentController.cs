﻿using Intelligent_Document_Processing_and_Analysis_API.Models.LLM;
using Intelligent_Document_Processing_and_Analysis_API.Services;
using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Globalization;

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

            CompletionRequest completionRequest = new(new(CompletionRequestsConstants.summaryCompletionRequest.Messages), CompletionRequestsConstants.summaryCompletionRequest.Temperature);

            completionRequest.Messages.Add(new(Globals.CompletionMessageRoleUser, text));

            var summary = await _llmCompletionService.GetCompletionAsync(completionRequest).ConfigureAwait(false);

            if (summary == null || summary.Choices == null || summary.Choices.Count == 0)
                return StatusCode(500, "An error occurred while getting the completion.");

            return Ok(summary.Choices[0].Message.Content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(DocumentController), nameof(GetDocumentSummary));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }

    [HttpPost("analysis")]
    public async Task<ActionResult<string>> GetDocumentAnalysis(IFormFile file)
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

            CompletionRequest completionRequest = new(new(CompletionRequestsConstants.analysisCompletionRequest.Messages), CompletionRequestsConstants.analysisCompletionRequest.Temperature);

            completionRequest.Messages.Add(new(Globals.CompletionMessageRoleUser, text));

            var analysis = await _llmCompletionService.GetCompletionAsync(completionRequest).ConfigureAwait(false);

            if (analysis == null || analysis.Choices == null || analysis.Choices.Count == 0)
                return StatusCode(500, "An error occurred while getting the completion.");

            return Ok(analysis.Choices[0].Message.Content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(DocumentController), nameof(GetDocumentAnalysis));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }

    [HttpPost("sentiment")]
    public async Task<ActionResult<decimal>> GetDocumentSentiment(IFormFile file)
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

            CompletionRequest completionRequest = new(new(CompletionRequestsConstants.sentimentCompletionRequest.Messages), CompletionRequestsConstants.sentimentCompletionRequest.Temperature);

            completionRequest.Messages.Add(new(Globals.CompletionMessageRoleUser, text));

            var sentiment = await _llmCompletionService.GetCompletionAsync(completionRequest).ConfigureAwait(false);

            if (sentiment == null || sentiment.Choices == null || sentiment.Choices.Count == 0)
                return StatusCode(500, "An error occurred while getting the completion.");

            string sentimentString = sentiment.Choices[0].Message.Content;

            if (!decimal.TryParse(sentimentString, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal sentimentValue))
                return StatusCode(500, "An error occurred while parsing the sentiment value.");

            return Ok(sentimentValue);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(DocumentController), nameof(GetDocumentSentiment));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }

    [HttpPost("dangerous")]
    public async Task<ActionResult<string>> GetDocumentDangerous(IFormFile file)
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

            CompletionRequest completionRequest = new(new(CompletionRequestsConstants.dangerousCompletionRequest.Messages), CompletionRequestsConstants.dangerousCompletionRequest.Temperature);

            completionRequest.Messages.Add(new(Globals.CompletionMessageRoleUser, text));

            var dangerous = await _llmCompletionService.GetCompletionAsync(completionRequest).ConfigureAwait(false);

            if (dangerous == null || dangerous.Choices == null || dangerous.Choices.Count == 0)
                return StatusCode(500, "An error occurred while getting the completion.");

            return Ok(dangerous.Choices[0].Message.Content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(DocumentController), nameof(GetDocumentDangerous));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }

    [HttpPost("compare")]
    public async Task<ActionResult<string>> GetDocumentComparison(IFormFile file1, IFormFile file2)
    {
        try
        {
            if (file1 == null || file1.Length == 0)
                return BadRequest("File1 is empty.");

            if (file2 == null || file2.Length == 0)
                return BadRequest("File2 is empty.");

            var fileExtension1 = Path.GetExtension(file1.FileName).ToLowerInvariant();

            if (!Globals.allowedDocumentExtensions.Contains(fileExtension1))
                return BadRequest($"File1 extension {fileExtension1} is not allowed. Only {Globals.allowedDocumentExtensionsString} are allowed.");

            var fileExtension2 = Path.GetExtension(file2.FileName).ToLowerInvariant();

            if (!Globals.allowedDocumentExtensions.Contains(fileExtension2))
                return BadRequest($"File2 extension {fileExtension2} is not allowed. Only {Globals.allowedDocumentExtensionsString} are allowed.");

            var filePath1 = await _saveFileService.SaveFileAsync(file1, fileExtension1).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(filePath1))
                return StatusCode(500, "An error occurred while saving the file1.");

            var filePath2 = await _saveFileService.SaveFileAsync(file2, fileExtension2).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(filePath2))
                return StatusCode(500, "An error occurred while saving the file2.");

            var text1 = _textExtractionService.ExtractText(filePath1, fileExtension1);

            if (string.IsNullOrWhiteSpace(text1))
                return StatusCode(500, "An error occurred while extracting text from the file1.");

            var text2 = _textExtractionService.ExtractText(filePath2, fileExtension2);

            if (string.IsNullOrWhiteSpace(text2))
                return StatusCode(500, "An error occurred while extracting text from the file2.");

            CompletionRequest completionRequest = new(new(CompletionRequestsConstants.compareCompletionRequest.Messages), CompletionRequestsConstants.compareCompletionRequest.Temperature);

            completionRequest.Messages.Add(new(Globals.CompletionMessageRoleUser, $"First document : {text1}"));
            completionRequest.Messages.Add(new(Globals.CompletionMessageRoleUser, $"Second document : {text2}"));

            var comparison = await _llmCompletionService.GetCompletionAsync(completionRequest).ConfigureAwait(false);

            if (comparison == null || comparison.Choices == null || comparison.Choices.Count == 0)
                return StatusCode(500, "An error occurred while getting the completion.");

            return Ok(comparison.Choices[0].Message.Content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(DocumentController), nameof(GetDocumentComparison));

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