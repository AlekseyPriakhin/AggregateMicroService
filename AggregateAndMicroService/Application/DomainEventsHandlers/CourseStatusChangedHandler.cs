using AggregateAndMicroService.Application.IntegrationEvents;
using AggregateAndMicroService.Infrastructure;

using MediatR;

public class CourseStatusChangedHandler : INotificationHandler<CourseStatusChanged>
{
    private readonly LearningContext _learningContext;
    private readonly ILogger<CourseStatusChangedHandler> _logger;
    private readonly IMediator _mediator;

    public CourseStatusChangedHandler(LearningContext learningContext, IMediator mediator, ILogger<CourseStatusChangedHandler> logger)
    {
        _learningContext = learningContext;
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Handle(CourseStatusChanged notification, CancellationToken cancellationToken)
    {
        var status = notification.Course.Status.Value.ToString();
        _logger.LogInformation($"Course {notification.Course.Id.Value}: status changed to {status}");

        var integrationEvent = new CourseStatusChangeIntegrationEvent
        {
            Status = notification.Course.Status.Value.ToString(),
            /* CourseId = notification.Course.Id.Value.ToString(),
            Title = notification.Course.Title,
            StagesCount = notification.Course.StageCount.Value, */
            //Description = notification.Course.Description,
            /* Data = new CourseIntegrationEventDto
            {
            } */
        };

        await _mediator.Publish(integrationEvent);
    }
}
