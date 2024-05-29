using AggregateAndMicroService.Domain.Course;

using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public record CourseStatusChangeIntegrationEvent : IntegrationEvent<CourseIntegrationEventDto>, INotification
{
    public required string Status { get; init; }

    /* public required string CourseId { get; init; }
    public required string Title { get; init; }
    public required int StagesCount { get; init; } */
    //public string? Description { get; init; }
    public CourseStatusChangeIntegrationEvent() : base()
    {
    }
}
