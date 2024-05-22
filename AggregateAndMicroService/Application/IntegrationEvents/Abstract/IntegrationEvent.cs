namespace AggregateAndMicroService.Application.IntegrationEvents;

public record IntegrationEvent<T>
{
    public Guid Id { get; private set; }
    public string Timestamp { get; private set; }
    public required T Data { get; init; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.Now.ToString();
    }
}
