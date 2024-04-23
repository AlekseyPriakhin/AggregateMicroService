using AggregateAndMicroService.Domain.CourseProgress;

using MediatR;

namespace AggregateAndMicroService.Domain.DomainEvents;

public record StageCompletingCreated : INotification
{
    StageCourseCompleting StageCourseCompleting { get; init; }

    public StageCompletingCreated(StageCourseCompleting stageCourseCompleting)
    {
        StageCourseCompleting = stageCourseCompleting;
    }
}
