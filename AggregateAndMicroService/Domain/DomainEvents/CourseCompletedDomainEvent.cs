using AggregateAndMicroService.Domain.CourseProgress;

using MediatR;

namespace AggregateAndMicroService.Domain.DomainEvents;

public record CourseCompletedDomainEvent : INotification
{
    public CourseCompleting CourseCompleting { get; init; }
    public CourseCompletedDomainEvent(CourseCompleting courseCompleting)
    {
        CourseCompleting = courseCompleting;
    }
}
