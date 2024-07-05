using Intelligent_Document_Processing_and_Analysis_API.Models.DTOs;

namespace Intelligent_Document_Processing_and_Analysis_API.Repositories;

public interface ILlmInteractionRepository
{
    Task<LlmInteractionDto> GetByIdAsync(int id);
    Task<IEnumerable<LlmInteractionDto>> GetAllAsync();
    Task<LlmInteractionDto> AddAsync(CreateLlmInteractionDto interactionDto);
    // Task<LlmInteractionDto> UpdateAsync(int id, UpdateLlmInteractionDto interactionDto);
    // Task<bool> DeleteAsync(int id);
    // Task<bool> ExistsAsync(int id);
}