using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    public interface IConnectionProvider: IDisposable
    {
        IConnection GetConnection();
    }
}