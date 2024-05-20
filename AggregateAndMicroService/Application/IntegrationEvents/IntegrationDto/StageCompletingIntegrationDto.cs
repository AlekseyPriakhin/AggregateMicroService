namespace AggregateAndMicroService.Application.IntegrationEvents;

public record StageCompletingIntegrationDto
{
    public required string Id { get; init; }
    public required string CourseCompletingId { get; init; }
    public required string UserId { get; init; }
    public required int Progress { get; init; }
}
