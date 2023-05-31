using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    public class ConnectionProvider : IConnectionProvider
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        public ConnectionProvider(ConnectionConfig config)
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri(config.Url),
            };
            if (config.ClientProvideName != null)
                _factory.ClientProvidedName = config.ClientProvideName;

            _connection = _factory.CreateConnection();
        }
        public IConnection GetConnection() => _connection;

 
    }
}
