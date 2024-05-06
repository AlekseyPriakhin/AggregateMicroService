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
            var ent = ctx.ChangeTracker.Entries<Entity<Guid>>().ToList();

            foreach (var entity in ent)
            {
                System.Console.WriteLine(entity.Entity.GetType());
            }

            System.Console.WriteLine(ent.Count());
            System.Console.WriteLine(ctx.ChangeTracker.Entries().Count());

            var domainEntities = ctx.ChangeTracker
                                    .Entries<Entity<object>>()
                                    .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

            var domainEvents = domainEntities
                                .SelectMany(x => x.Entity.DomainEvents)
                                .ToList();

            domainEntities.ToList()
                          .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
