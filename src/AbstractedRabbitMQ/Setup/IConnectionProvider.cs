using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    public interface IConnectionProvider
    {
        void Dispose();
        IConnection GetConnection();
    }
}