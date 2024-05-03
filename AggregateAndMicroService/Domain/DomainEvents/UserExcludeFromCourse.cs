using AggregateAndMicroService.Domain.CourseProgress;

using MediatR;

public record UserExcludeFromCourse : INotification
{
    public Guid UserId { get; init; }
    public CourseCompleting CourseCompleting { get; init; }

    public IEnumerable<StageCourseCompleting> Stages { get; init; }

    public UserExcludeFromCourse(Guid userId, CourseCompleting courseCompleting, IEnumerable<StageCourseCompleting> stages)
    {
        UserId = userId;
        CourseCompleting = courseCompleting;
        Stages = stages;
    }
}
