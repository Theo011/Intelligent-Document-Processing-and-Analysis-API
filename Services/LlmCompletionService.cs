using Intelligent_Document_Processing_and_Analysis_API.Models.DTOs;
using Intelligent_Document_Processing_and_Analysis_API.Models.LLM;
using Intelligent_Document_Processing_and_Analysis_API.Repositories;
using Intelligent_Document_Processing_and_Analysis_API.Utilities;
using Newtonsoft.Json;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Services;

public class LlmCompletionService(ILlmInteractionRepository llmInteractionRepository, IHttpClientFactory httpClientFactory) : ILlmCompletionService
{
    private readonly ILlmInteractionRepository _llmInteractionRepository = llmInteractionRepository ?? throw new ArgumentNullException(nameof(llmInteractionRepository));
    private readonly HttpClient _httpClient = httpClientFactory?.CreateClient(Globals.CompletionHttpClientName) ?? throw new ArgumentNullException(nameof(httpClientFactory));

    public async Task<CompletionResponse> GetCompletionAsync(CompletionRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(AppSettingsConstants.LLM_COMPLETION_ENDPOINT, request).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            await _llmInteractionRepository.AddAsync(new CreateLlmInteractionDto
            {
                Input = JsonConvert.SerializeObject(request) ?? throw new ArgumentNullException(nameof(request)),
                Output = responseContent
            }).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<CompletionResponse>(responseContent)
                ?? throw new($"Failed to deserialize completion response at class: {nameof(LlmCompletionService)}, method: {nameof(GetCompletionAsync)}.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(LlmCompletionService), nameof(GetCompletionAsync));

            throw;
        }
    }
}