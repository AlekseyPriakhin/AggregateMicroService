using AggregateAndMicroService.Infrastructure.Kafka;

using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;


public record CourseStatusChangeIntegrationEventHandler : INotificationHandler<CourseStatusChangeIntegrationEvent>
{
    private readonly KafkaService _broker;

    public CourseStatusChangeIntegrationEventHandler(KafkaService broker)
    {
        _broker = broker;
    }

    public async Task Handle(CourseStatusChangeIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var message = System.Text.Json.JsonSerializer.Serialize(notification);
        await _broker.ProduceAsync("course", message);
    }

}