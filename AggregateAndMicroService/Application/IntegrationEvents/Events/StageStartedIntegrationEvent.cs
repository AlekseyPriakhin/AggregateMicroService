using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public record StageStartedIntegrationEvent : IntegrationEvent<StageCompletingIntegrationDto>, INotification
{
}
