// See https://aka.ms/new-console-template for more information
using System.Text.Json;

using Confluent.Kafka;

Console.WriteLine("Hello, World!");



var config = new ConsumerConfig
{
    BootstrapServers = "broker:9092",
    GroupId = "test-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};



using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
{
    consumer.Subscribe("course");
    while (true)
    {
        var message = consumer.Consume();
        var deserializedMessage = new Object();

        switch (message.Topic)
        {
            case "course":
                {
                    deserializedMessage = JsonSerializer.Deserialize<CourseStatusChangeIntegrationEvent>(message.Message.Value);
                    break;
                }

            default: break;
        }

        System.Console.WriteLine(deserializedMessage);

    }

}

public class CourseStatusChangeIntegrationEvent
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Status { get; init; }
    public required int StagesCount { get; init; }
    public string? Description { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
    //public List<StageResponseDto> Stages { get; init; } = [];
}
