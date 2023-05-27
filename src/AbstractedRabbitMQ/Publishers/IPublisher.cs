namespace AbstractedRabbitMQ.Publishers
{
    public interface IPublisher:IDisposable
    {
        void Publish(string message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration);
    }
}