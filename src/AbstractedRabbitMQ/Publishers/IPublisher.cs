namespace AbstractedRabbitMQ.Publishers
{
    public interface IPublisher:IDisposable
    {
        Task Publish(string message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration);
        Task Publish<T>(T message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration);
    }
}