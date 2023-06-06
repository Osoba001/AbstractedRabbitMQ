using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    internal class ConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
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
        public IModel GetModel()=>_connection.CreateModel();
 
    }
}
