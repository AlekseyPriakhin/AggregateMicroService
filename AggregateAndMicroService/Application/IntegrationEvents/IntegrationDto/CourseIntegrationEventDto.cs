namespace AggregateAndMicroService.Application.IntegrationEvents;


public record CourseIntegrationEventDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Status { get; init; }
    public required int StagesCount { get; init; }
    public string? Description { get; init; }
}
