using Confluent.Kafka;

namespace AggregateAndMicroService.Infrastructure.Kafka;

public class KafkaService
{
    private IProducer<Null, string> _producer;

    public KafkaService(IConfiguration configuration)
    {
        var brokerConfig = new ProducerConfig
        {
            BootstrapServers = configuration["broker"],
            AllowAutoCreateTopics = true,

        };
        _producer = new ProducerBuilder<Null, string>(brokerConfig).Build();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
    }

}
