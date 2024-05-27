// See https://aka.ms/new-console-template for more information
using System.Text.Json;

using Confluent.Kafka;

using Newtonsoft.Json.Linq;




var address = Environment.GetEnvironmentVariable("BROKER_URL") ?? "localhost:19092";

System.Console.WriteLine(address);

Console.WriteLine("Broker Consumer Started" + '\n');
var config = new ConsumerConfig
{
    BootstrapServers = address, //localhost:19092 || broker:9092
    GroupId = "test-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};



using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
{
    consumer.Subscribe(["course"]);

    while (true)
    {
        var message = consumer.Consume();
        var brokerMessage = JsonSerializer.Deserialize<BrokerMessage>(message.Message.Value);
        if (brokerMessage is not null)
        {
            var deserializedMessage = JObject.Parse(message.Message.Value);
            Console.WriteLine($"MESSAGE: Topic - {message.Topic}");
            foreach (var pair in deserializedMessage)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
            Console.WriteLine('\n');
        }


    }
}


public class BrokerMessage
{
    public string Id { get; set; }
    public string Timestamp { get; set; }
    public object Data { get; set; }
}
