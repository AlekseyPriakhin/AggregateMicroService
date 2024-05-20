using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public record CourseStartedIntegrationEvent : INotification
{
    public string Id { get; init; }
    public string UserId { get; init; }

    public string CourseId { get; init; }

    public string Status { get; init; }

    public int Progress { get; init; }

    public int StagesCountData { get; init; }
}
