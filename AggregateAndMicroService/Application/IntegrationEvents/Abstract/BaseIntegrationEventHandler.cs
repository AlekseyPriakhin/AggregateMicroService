using AggregateAndMicroService.Infrastructure.Kafka;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public abstract class BaseIntegrationEventHandler
{

    protected readonly KafkaService _broker;

    public BaseIntegrationEventHandler(KafkaService kafkaService)
    {
        _broker = kafkaService;
    }
}
