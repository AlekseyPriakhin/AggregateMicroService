using AggregateAndMicroService.Infrastructure.Kafka;

using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public class CourseStartedIntegrationEventHandler : BaseIntegrationEventHandler, INotificationHandler<CourseStartedIntegrationEvent>
{
    public CourseStartedIntegrationEventHandler(KafkaService kafkaService) : base(kafkaService)
    {
    }

    public async Task Handle(CourseStartedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var message = System.Text.Json.JsonSerializer.Serialize(notification);
        await _broker.ProduceAsync("course_completing", message);
    }
}
