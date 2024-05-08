using AggregateAndMicroService.Domain.DomainEvents;
using AggregateAndMicroService.Domain.User;
using AggregateAndMicroService.Infrastructure;

using MediatR;

namespace AggregateAndMicroService.Application.DomainEventHandlers;

public class CourseStartedDomainEventHandler : INotificationHandler<CourseStartedDomainEvent>
{
    private readonly LearningContext _context;
    private readonly ILogger<CourseStartedDomainEventHandler> _logger;

    public CourseStartedDomainEventHandler(LearningContext context, ILogger<CourseStartedDomainEventHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task Handle(CourseStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var userId = UserId.Of(notification.CourseCompleting.UserId);
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception($"User with id {userId.Value} not found");
        user.UpdateCourseInProgressCount(true);

        await _context.AddAsync(notification.CourseCompleting, cancellationToken);
        _logger.LogInformation($"Course {notification.CourseCompleting.Id.Value} started by user {userId.Value}");

        await _context.AddAsync(notification.FirstStageCompleting, cancellationToken);
        _logger.LogInformation($"Stage {notification.FirstStageCompleting.StageId} started by user {userId.Value}");

    }

}
