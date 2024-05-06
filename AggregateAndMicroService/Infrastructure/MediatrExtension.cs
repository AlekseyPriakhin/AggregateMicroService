using AggregateAndMicroService.Common;
using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Infrastructure;

using MediatR;

using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Ordering.Infrastructure
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, LearningContext ctx)
        {
            var entities = ctx.ChangeTracker.Entries()
                        .Where(e => e.Entity is IDomainEventGenerator)
                        .Select(e => (IDomainEventGenerator)e.Entity)
                        .Where(e => e.DomainEvents != null && e.DomainEvents.Count > 0)
                        .ToList();


            var domainEvents = entities
                                .SelectMany(x => x.DomainEvents)
                                .ToList();

            entities.ForEach(entity => entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
