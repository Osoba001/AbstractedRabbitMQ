using RabbitMQ.Client;

namespace AbstractedRabbitMQ.Publishers
{
    public class PublisherConfig
    {
        public PublisherConfig()
        {
            durable= true;
            timeToLive=TimeSpan.FromMinutes(1);
            exchange = "default-direct-exchange";
            exchangeType = ExchangeType.Direct;
        }
        public string exchange { get; set; }
        public string exchangeType { get; set; }
        public TimeSpan timeToLive { get; set; }
        public bool durable { get; set;}
        public bool autodelete { get; set; }
    }
}
