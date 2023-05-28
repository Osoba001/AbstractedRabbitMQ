using AbstractedRabbitMQ.Setup;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace AbstractedRabbitMQ.Publishers
{
    public class Publisher : IPublisher
    {
        private readonly string exchangeName;
        private readonly IModel _model;
        private bool _disposed;

        public Publisher(IConnectionProvider connectionProvier, PublisherConfig config)
        {
            this.exchangeName = config.exchange;
            _model = connectionProvier.GetConnection().CreateModel();
            var ttl = new Dictionary<string, object> { { "x-message-ttl", config.timeToLive.TotalMilliseconds } };
            _model.ExchangeDeclare(exchangeName, config.exchangeType.ToString(), config.durable, config.autodelete, ttl);

        }
        public Task Publish(string message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration)
        {
            var MessageInByte = Encoding.UTF8.GetBytes(message);
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;
            if (messageAttribute != null)
                properties.Headers = messageAttribute;
            if (expiration != null)
                properties.Expiration = expiration?.TotalMilliseconds.ToString();

            _model.BasicPublish(exchangeName, routingKey, properties, MessageInByte);
            return Task.CompletedTask;
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

        public Task Publish<T>(T message, string routingKey, IDictionary<string, object>? messageAttribute, TimeSpan? expiration)
        {
            var messageString=JsonConvert.SerializeObject(message);
            var MessageInByte = Encoding.UTF8.GetBytes(messageString);
            var properties = _model.CreateBasicProperties();
            properties.Persistent = true;
            if (messageAttribute != null)
                properties.Headers = messageAttribute;
            if (expiration != null)
                properties.Expiration = expiration?.TotalMilliseconds.ToString();

            _model.BasicPublish(exchangeName, routingKey, properties, MessageInByte);
            return Task.CompletedTask;
        }
    }
}
