using Intelligent_Document_Processing_and_Analysis_API.Models.LLM;
using Intelligent_Document_Processing_and_Analysis_API.Services;
using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LlmController(ILlmCompletionService llmCompletionService) : ControllerBase
{
    private readonly ILlmCompletionService _llmCompletionService = llmCompletionService ?? throw new ArgumentNullException(nameof(llmCompletionService));

    [HttpPost("completion")]
    public async Task<ActionResult<string>> GetCompletion([FromBody] CompletionRequest request)
    {
        try
        {
            var response = await _llmCompletionService.GetCompletionAsync(request).ConfigureAwait(false);

            return Ok(response.Choices[0].Message.Content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(LlmController), nameof(GetCompletion));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }

    [HttpPost("testcompletion")]
    public async Task<ActionResult<string>> GetTestCompletion()
    {
        try
        {
            var response = await _llmCompletionService.GetCompletionAsync(CompletionRequestsConstants.testCompletionRequest).ConfigureAwait(false);

            return Ok(response.Choices[0].Message.Content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(LlmController), nameof(GetTestCompletion));

            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }
}