using AggregateAndMicroService.Application.DTO.Response;
using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Application.Mappers;

public static class StageMapper
{
    public static StageResponseDto ToStageResponseDto(Stage stage)
    {
        return new StageResponseDto
        {
            Id = stage.Id.Value.ToString(),
            Title = stage.Title,
            Type = stage.Type.Value.ToString(),
            Duration = stage.Duration.Value,
            CourseId = stage.CourseId.Value.ToString(),
            Previous = stage.Previous?.ToString()
        };
    }
}
