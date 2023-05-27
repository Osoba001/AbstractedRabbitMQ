using AbstractedRabbitMQ.Setup;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractedRabbitMQ.Subscribers
{
    public class Subscriber : ISubscriber
    {
        private readonly string queue;
        private readonly IModel model;
        private bool _disposed;

        public Subscriber(IConnectionProvider connectionProvider, string exchange, string exchangeType, string queue, string routingKey, TimeSpan? timeToLive, ushort prefetchCount = 5, bool durable = true, bool exclusive = false, bool autodelete = false)
        {
            this.queue = queue;
            model = connectionProvider.GetConnection().CreateModel();
            timeToLive??= TimeSpan.FromMinutes(1);
            var ttl = new Dictionary<string, object> { { "x-message-ttl", timeToLive.Value.TotalMilliseconds } };
            model.ExchangeDeclare(exchange, exchangeType, arguments: ttl);
            model.QueueDeclare(queue, durable, exclusive, autodelete, ttl);
            model.QueueBind(queue, exchange, routingKey);
            model.BasicQos(0, prefetchCount: prefetchCount, global: false);
        }
        public Subscriber(IConnectionProvider connectionProvider, string exchange, string exchangeType, string queue, string routingKey, TimeSpan? timeToLive, ushort prefetchCount = 5)
            : this(connectionProvider, exchange, exchangeType, queue, routingKey, timeToLive, prefetchCount, true, false, false) { }

        public void Subscribe(Func<string, IDictionary<string, object>, bool> callBack)
        {
            var consumer = new EventingBasicConsumer(model);

            consumer.Received += (sender, e) =>
            {
                var bodyInBitArray = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(bodyInBitArray);
                bool success = callBack(message, e.BasicProperties.Headers);
                if (success)
                {
                    model.BasicAck(e.DeliveryTag, true);
                }
            };
            model.BasicConsume(queue, false, consumer);
        }
        public void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callBack)
        {
            var consumer = new EventingBasicConsumer(model);

            consumer.Received += async (sender, e) =>
            {
                var bodyInBitArray = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(bodyInBitArray);
                bool success = await callBack(message, e.BasicProperties.Headers);
                if (success)
                {
                    model.BasicAck(e.DeliveryTag, true);
                }
            };
            model.BasicConsume(queue, false, consumer);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                model?.Close();

            _disposed = true;
        }

        
    }
}
