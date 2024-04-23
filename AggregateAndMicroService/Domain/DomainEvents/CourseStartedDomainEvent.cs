using AggregateAndMicroService.Domain.Course;

using MediatR;

namespace AggregateAndMicroService.Domain.DomainEvents;


public record CourseStartedDomainEvent : INotification
{
    public CourseCompleting CourseCompleting { get; init; }
    public CourseStartedDomainEvent(CourseCompleting courseCompleting)
    {
        CourseCompleting = courseCompleting;
    }
}