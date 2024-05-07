using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Application.DTO.Response;


public record ResponseCourseDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required Statuses Status { get; init; }
    public required int StagesCount { get; init; }
    public string Description { get; init; } = String.Empty;
    public List<StageResponseDto> Stages { get; init; } = [];
}
