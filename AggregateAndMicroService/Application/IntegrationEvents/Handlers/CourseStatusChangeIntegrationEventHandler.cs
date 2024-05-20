using AggregateAndMicroService.Infrastructure.Kafka;

using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;


public class CourseStatusChangeIntegrationEventHandler : BaseIntegrationEventHandler, INotificationHandler<CourseStatusChangeIntegrationEvent>
{
    public CourseStatusChangeIntegrationEventHandler(KafkaService broker) : base(broker)
    {
    }

    public async Task Handle(CourseStatusChangeIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var message = System.Text.Json.JsonSerializer.Serialize(notification);
        await _broker.ProduceAsync("course", message);
    }

}
