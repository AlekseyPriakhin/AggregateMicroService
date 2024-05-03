using AggregateAndMicroService.Infrastructure;

using MediatR;

public class CourseStatusChangedToArchivedHandler : INotificationHandler<CourseStatusChangedToArchived>
{
    private readonly LearningContext _learningContext;

    public CourseStatusChangedToArchivedHandler(LearningContext learningContext)
    {
        _learningContext = learningContext;
    }
    public Task Handle(CourseStatusChangedToArchived notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
