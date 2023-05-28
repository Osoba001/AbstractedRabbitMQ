using AbstractedRabbitMQ.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractedRabbitMQ.Subscribers
{
    public class SubScribeConfig
    {
        public SubScribeConfig()
        {
            durable = true;
            timeToLive = TimeSpan.FromMinutes(1);
            exchange = "default-direct-exchange";
            exchangeType = ExchangeTypeRMQ.direct;
            queue = "default-queue";
            routingKey = "default-routingkey";
            prefetchCount= 5;
            prefetchSize = 0;
        }
        public string exchange { get; set; }
        public ExchangeTypeRMQ exchangeType { get; set; }
        public string queue { get; set; }
        public string routingKey { get; set; }
        public TimeSpan timeToLive { get; set; }
        public ushort prefetchCount { get; set; }
        public ushort prefetchSize { get; set; }
        public bool durable { get; set; }
        public bool exclusive { get; set; }
        public bool autodelete { get; set; }
        public bool global { get; set; }
    }
}
