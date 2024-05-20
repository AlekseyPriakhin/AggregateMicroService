using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public record CourseStartedIntegrationEvent : IntegrationEvent<CourseCompletingIntegrationEventDto>, INotification
{

}
