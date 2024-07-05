using AutoMapper;
using Intelligent_Document_Processing_and_Analysis_API.Models.DTOs;
using Intelligent_Document_Processing_and_Analysis_API.Models.Entities;
using Serilog;

namespace Intelligent_Document_Processing_and_Analysis_API.Profiles;

public class LlmInteractionProfile : Profile
{
    public LlmInteractionProfile()
    {
        try
        {
            // Entity to DTO
            CreateMap<LlmInteraction, LlmInteractionDto>();

            // CreateDTO to Entity
            CreateMap<CreateLlmInteractionDto, LlmInteraction>()
                .ConstructUsing(src => new(src.Input, src.Output, src.Metadata));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error at class: {class}, method: {method}", nameof(LlmInteractionProfile), nameof(LlmInteractionProfile));
        }
    }
}