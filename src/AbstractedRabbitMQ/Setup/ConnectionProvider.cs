using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Setup
{
    public class ConnectionProvider : IConnectionProvider
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private bool _disposed;
        public ConnectionProvider(string url, string? clientProvidedName)
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri(url),
            };
            if (clientProvidedName != null)
                _factory.ClientProvidedName = clientProvidedName;

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
