namespace AggregateAndMicroService.Application.IntegrationEvents;


public record CourseCompletingIntegrationEventDto
{

    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string CourseId { get; init; }
    public required string Status { get; init; }
    public required int Progress { get; init; }
    public required int StagesCountData { get; init; }

}
