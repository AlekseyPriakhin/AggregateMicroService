using AggregateAndMicroService.Domain.CourseProgress;
using AggregateAndMicroService.Infrastructure;

using MediatR;

public class CourseStagesUpdatedHandler : INotificationHandler<CourseStageUpdated>
{
    private readonly LearningContext _context;

    public CourseStagesUpdatedHandler(LearningContext context)
    {
        _context = context;
    }

    public Task Handle(CourseStageUpdated notification, CancellationToken cancellationToken)
    {
        var courseId = notification.Course.Id.Value;

        return Task.CompletedTask;
    }

}
