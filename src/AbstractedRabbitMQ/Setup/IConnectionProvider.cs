using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}