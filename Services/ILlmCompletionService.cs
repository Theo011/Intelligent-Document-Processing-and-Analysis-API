using Intelligent_Document_Processing_and_Analysis_API.Models.LLM;

namespace Intelligent_Document_Processing_and_Analysis_API.Services;

public interface ILlmCompletionService
{
    Task<CompletionResponse> GetCompletionAsync(CompletionRequest request);
}