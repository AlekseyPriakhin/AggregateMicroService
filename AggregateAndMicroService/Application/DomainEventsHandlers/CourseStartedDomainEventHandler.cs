using AggregateAndMicroService.Domain.DomainEvents;
using AggregateAndMicroService.Infrastructure;

using MediatR;

namespace AggregateAndMicroService.Application.DomainEventHandlers;

public class CourseStartedDomainEventHandler : INotificationHandler<CourseStartedDomainEvent>
{
    private readonly LearningContext _context;

    public CourseStartedDomainEventHandler(LearningContext context)
    {
        _context = context;
    }
    public async Task Handle(CourseStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        var userId = notification.CourseCompleting.UserId;
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception($"User with id {userId} not found");
        user.UpdateCourseInProgressCount(true);
        await _context.AddAsync(notification.CourseCompleting);
    }

}
