using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public record StageStartedIntegrationEvent : INotification
{
    public string Id { get; init; }
    public string CourseCompletingId { get; init; }

    public string UserId { get; init; }
    public int Progress { get; init; }

}
