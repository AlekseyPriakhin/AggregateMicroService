using AggregateAndMicroService.Infrastructure.Kafka;

using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public class StageStartedIntegrationEventHandler : BaseIntegrationEventHandler, INotificationHandler<StageStartedIntegrationEvent>
{
    public StageStartedIntegrationEventHandler(KafkaService kafkaService) : base(kafkaService)
    {
    }

    public async Task Handle(StageStartedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var message = System.Text.Json.JsonSerializer.Serialize(notification);
        await _broker.ProduceAsync("stage_completing", message);
    }
}
