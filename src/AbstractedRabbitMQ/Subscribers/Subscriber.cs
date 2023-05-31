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

        public Subscriber(IConnectionProvider connectionProvider, SubScribeConfig config)
        {
            queue = config.queue;
            model = connectionProvider.GetConnection().CreateModel();
            var ttl = new Dictionary<string, object> { { "x-message-ttl", config.timeToLive.TotalMilliseconds } };
            model.ExchangeDeclare(config.exchange, config.exchangeType, arguments: ttl);
            model.QueueDeclare(queue, config.durable, config.exclusive, config.autodelete, ttl);
            model.QueueBind(queue, config.exchange, config.routingKey);
            model.BasicQos(config.prefetchSize, prefetchCount: config.prefetchCount, global: config.global);
        }
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
            model.BasicConsume(queue, false,consumer:consumer);
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

        public Task SubscribeAsync<T>(Func<SubResult<T>, IDictionary<string, object>, Task<bool>> callBack)
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
