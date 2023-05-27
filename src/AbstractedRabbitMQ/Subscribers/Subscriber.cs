using AbstractedRabbitMQ.Setup;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

        public Task Subscribe(Func<string, IDictionary<string, object>, bool> callBack)
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
            return Task.CompletedTask;
        }
        public Task SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callBack)
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
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                model?.Close();

            _disposed = true;
        }

        public Task Subscribe<T>(Func<SubResult<T>, IDictionary<string, object>, bool> callBack)
        {
            var consumer = new EventingBasicConsumer(model);

            consumer.Received += (sender, e) =>
            {
                SubResult<T> res = new();
                var bodyInBitArray = e.Body.ToArray();
                try
                {
                    var message = Encoding.UTF8.GetString(bodyInBitArray);
                    var result = JsonConvert.DeserializeObject<T>(message);
                    if (result == null)
                        res.AddError($"Unable to convert the response message to type of {typeof(T)}");
                    else
                        res.Value= result;
                    bool success = callBack(res, e.BasicProperties.Headers);
                    if (success)
                        model.BasicAck(e.DeliveryTag, true);

                }
                catch (Exception ex)
                {
                    res.AddError(ex.Message);
                }
                
            };
            model.BasicConsume(queue, false, consumer);
            return Task.CompletedTask;
        }

        public Task SubscribeAsyc<T>(Func<SubResult<T>, IDictionary<string, object>, Task<bool>> callBack)
        {
            var consumer = new EventingBasicConsumer(model);

            consumer.Received += async (sender, e) =>
            {
                SubResult<T> res = new();
                var bodyInBitArray = e.Body.ToArray();
                try
                {
                    var message = Encoding.UTF8.GetString(bodyInBitArray);
                    var result = JsonConvert.DeserializeObject<T>(message);
                    if (result == null)
                        res.AddError($"Unable to convert the response message to type of {typeof(T)}");
                    else
                        res.Value = result;
                    bool success = await callBack(res, e.BasicProperties.Headers);
                    if (success)
                        model.BasicAck(e.DeliveryTag, true);

                }
                catch (Exception ex)
                {
                    res.AddError(ex.Message);

                }

            };
            model.BasicConsume(queue, false, consumer);
            return Task.CompletedTask;
        }
    }
}
