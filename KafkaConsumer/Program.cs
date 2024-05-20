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
    consumer.Subscribe(["course", "course_completing", "stage_completing"]);

    while (true)
    {
        try
        {
            var message = consumer.Consume();
            var deserializedMessage = JObject.Parse(message.Message.Value);
            if (deserializedMessage is not null)
            {
                System.Console.WriteLine($"MESSAGE: Topic - {message.Topic}");
                foreach (var pair in deserializedMessage)
                {
                    System.Console.WriteLine($"{pair.Key}: {pair.Value}");
                }
                System.Console.WriteLine('\n');
            }


        }
        catch (System.Exception err)
        {
            System.Console.WriteLine($"ERROR: {err}");
            Thread.Sleep(30000);
        }

    }

}


public class BrokerMessage
{
    string Id { get; set; }

    string Data { get; set; }
}
