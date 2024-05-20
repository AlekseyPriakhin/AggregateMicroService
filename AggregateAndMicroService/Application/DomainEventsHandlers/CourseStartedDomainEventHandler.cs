using AggregateAndMicroService.Application.IntegrationEvents;
using AggregateAndMicroService.Domain.DomainEvents;
using AggregateAndMicroService.Domain.User;
using AggregateAndMicroService.Infrastructure;

using MediatR;

namespace AggregateAndMicroService.Application.DomainEventHandlers;

public class CourseStartedDomainEventHandler : INotificationHandler<CourseStartedDomainEvent>
{
    private readonly LearningContext _context;
    private readonly ILogger<CourseStartedDomainEventHandler> _logger;

    private readonly IMediator _mediator;

    public CourseStartedDomainEventHandler(LearningContext context, IMediator mediator, ILogger<CourseStartedDomainEventHandler> logger)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }
    public async Task Handle(CourseStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var userId = UserId.Of(notification.CourseCompleting.UserId);
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception($"User with id {userId.Value} not found");
        user.UpdateCourseInProgressCount(true);

        await _context.AddAsync(notification.CourseCompleting, cancellationToken);
        _logger.LogInformation($"Course {notification.CourseCompleting.Id.Value} started by user {userId.Value}");

        var courseStartedEvent = new CourseStartedIntegrationEvent
        {
            Id = notification.CourseCompleting.Id.Value.ToString(),
            CourseId = notification.CourseCompleting.CourseId.ToString(),
            UserId = notification.CourseCompleting.UserId.ToString(),
            Status = notification.CourseCompleting.Status.Value.ToString(),
            Progress = notification.CourseCompleting.Progress.Value,
            StagesCountData = notification.CourseCompleting.Progress.Value
        };
        await _mediator.Publish(courseStartedEvent, cancellationToken);

        var stageStartedEvent = new StageStartedIntegrationEvent
        {
            Id = notification.FirstStageCompleting.Id.Value.ToString(),
            CourseCompletingId = notification.FirstStageCompleting.CourseCompletingId.Value.ToString(),
            UserId = notification.CourseCompleting.UserId.ToString(),
            Progress = notification.FirstStageCompleting.StageProgress.Value
        };
        await _mediator.Publish(stageStartedEvent, cancellationToken);

        await _context.AddAsync(notification.FirstStageCompleting, cancellationToken);
        _logger.LogInformation($"Stage {notification.FirstStageCompleting.StageId} started by user {userId.Value}");

    }

}
