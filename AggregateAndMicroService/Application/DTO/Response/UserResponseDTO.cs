namespace AggregateAndMicroService.Application.DTO.Response;


public record UserResponseDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required int CompletedCoursesCount { get; init; }
    public required int CourseInProgressCount { get; init; }
}
