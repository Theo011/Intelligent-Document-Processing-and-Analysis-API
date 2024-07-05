using AutoMapper;
using Intelligent_Document_Processing_and_Analysis_API.DbContexts;
using Intelligent_Document_Processing_and_Analysis_API.Models.DTOs;
using Intelligent_Document_Processing_and_Analysis_API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Repositories;

public class LlmInteractionRepository(SQLiteDbContext context, IMapper mapper) : ILlmInteractionRepository
{
    private readonly SQLiteDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<LlmInteractionDto> GetByIdAsync(int id)
    {
        try
        {
            var interaction = await _context.LlmInteractions.FindAsync(id).ConfigureAwait(false);

            return _mapper.Map<LlmInteractionDto>(interaction);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(LlmInteractionRepository), nameof(GetByIdAsync));

            throw;
        }
    }

    public async Task<IEnumerable<LlmInteractionDto>> GetAllAsync()
    {
        try
        {
            var interactions = await _context.LlmInteractions.ToListAsync().ConfigureAwait(false);

            return _mapper.Map<IEnumerable<LlmInteractionDto>>(interactions);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(LlmInteractionRepository), nameof(GetAllAsync));

            throw;
        }
    }

    public async Task<LlmInteractionDto> AddAsync(CreateLlmInteractionDto interactionDto)
    {
        try
        {
            var interaction = _mapper.Map<LlmInteraction>(interactionDto);

            _context.LlmInteractions.Add(interaction);

            if (await _context.SaveChangesAsync().ConfigureAwait(false) < 1)
                throw new($"Failed to save changes to the database at class: {nameof(LlmInteractionRepository)}, method: {nameof(AddAsync)} with input: {interactionDto.ToString}.");
            
            return _mapper.Map<LlmInteractionDto>(interaction);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(LlmInteractionRepository), nameof(AddAsync));

            throw;
        }
    }
}