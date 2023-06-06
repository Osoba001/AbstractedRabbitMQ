using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    public interface IConnectionProvider
    {
        IModel GetModel();
    }
}