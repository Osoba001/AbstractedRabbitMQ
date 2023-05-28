using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    public class ConnectionProvider : IConnectionProvider
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private bool _disposed;
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                _connection.Dispose();
            _disposed = true;
        }
    }
}
