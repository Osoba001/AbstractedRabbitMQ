using AbstractedRabbitMQ.Setup;
using RabbitMQ.Client;
using System.Text;

namespace AbstractedRabbitMQ.Publishers
{
    public class Publisher : IPublisher
    {
        private readonly string exchangeName;
        private readonly IModel _model;
        private bool _disposed;

        public Publisher(IConnectionProvider connectionProvier, string exchangeName, string exchangeType, TimeSpan? timeToLive,
           bool durable = false, bool autodelete = false)
        {
            this.exchangeName = exchangeName;
            _model = connectionProvier.GetConnection().CreateModel();

            timeToLive = timeToLive ?? TimeSpan.FromMinutes(1);
            var ttl = new Dictionary<string, object> { { "x-message-ttl", timeToLive.Value.TotalMilliseconds } };
            _model.ExchangeDeclare(exchangeName, exchangeType, durable, autodelete, ttl);

        }
        public Publisher(IConnectionProvider connectionProvier, string exchangeName, string exchangeType, TimeSpan? timeToLive)
            : this(connectionProvier, exchangeName, exchangeType, timeToLive, false, false) { }

        public void Publish(string message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration)
        {
            var MessageInByte = Encoding.UTF8.GetBytes(message);
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;
            if (messageAttribute != null)
                properties.Headers = messageAttribute;
            if (expiration != null)
                properties.Expiration = expiration?.TotalMilliseconds.ToString();

            _model.BasicPublish(exchangeName, routingKey, properties, MessageInByte);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                _model.Close();
            _disposed = true;
        }
    }
}
