using AggregateAndMicroService.Infrastructure;

using MediatR;

public class CourseStatusChangedHandler : INotificationHandler<CourseStatusChanged>
{
    private readonly LearningContext _learningContext;
    private readonly ILogger<CourseStatusChangedHandler> _logger;

    public CourseStatusChangedHandler(LearningContext learningContext, ILogger<CourseStatusChangedHandler> logger)
    {
        _learningContext = learningContext;
        _logger = logger;
    }
    public Task Handle(CourseStatusChanged notification, CancellationToken cancellationToken)
    {
        var status = notification.Course.Status.Value.ToString();
        _logger.LogInformation($"Course {notification.Course.Id.Value}: status changed to {status}");
        return Task.CompletedTask;
    }
}
