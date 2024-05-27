namespace AggregateAndMicroService.Application.IntegrationEvents;

public static class IntegrationEventBuilder
{
    public static IntegrationEvent<U> Build<T, U>(U data) where T : IntegrationEvent<U>
    {
        return new IntegrationEvent<U>
        {
            /* Data = data */
        };
    }
}
