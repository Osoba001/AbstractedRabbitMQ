namespace AbstractedRabbitMQ.Publishers
{
    public interface IPublisher
    {
        Task PublishAsync(string message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration);
        Task PublishAsync<T>(T message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration);
        void Publish<T>(T message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration);
        void Publish(string message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration);
    }
}