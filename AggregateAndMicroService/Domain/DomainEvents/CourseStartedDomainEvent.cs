using AggregateAndMicroService.Domain.CourseProgress;

using MediatR;

namespace AggregateAndMicroService.Domain.DomainEvents;


public record CourseStartedDomainEvent : INotification
{
    public CourseCompleting CourseCompleting { get; init; }

    public StageCourseCompleting FirstStageCompleting { get; init; }

    public CourseStartedDomainEvent(CourseCompleting courseCompleting, StageCourseCompleting firstStageCompleting)
    {
        CourseCompleting = courseCompleting;
        FirstStageCompleting = firstStageCompleting;
    }
}
