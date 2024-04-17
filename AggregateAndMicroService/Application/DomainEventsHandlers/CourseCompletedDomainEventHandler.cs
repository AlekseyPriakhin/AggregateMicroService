using AggregateAndMicroService.Domain.DomainEvents;
using AggregateAndMicroService.Infrastructure;

using MediatR;

namespace AggregateAndMicroService.Application.DomainEventHandlers;

public class CourseCompletedDomainEventHandler : INotificationHandler<CourseCompletedDomainEvent>
{
    private readonly LearningContext _context;
    public CourseCompletedDomainEventHandler(LearningContext learningContext)
    {
        _context = learningContext;
    }

    public async Task Handle(CourseCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var userId = notification.CourseCompleting.UserId;
        var user = await _context.Users.FindAsync(userId) ?? throw new Exception($"User with id {userId} not found");
        //await _context.SaveChangesAsync(cancellationToken); TODO Сделать отложенную обработку доменных событий
    }
}
