using AbstractedRabbitMQ.Publishers;
using AbstractedRabbitMQ.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace AbstractedRabbitMQ.Setup
{
    public static class ConfigureService
    {
        public static IServiceCollection AddRabbitMQConnection(this IServiceCollection services, string url,string? clientProvidedName)
        {
            services.AddScoped<IConnectionProvider>(x => new ConnectionProvider(url, clientProvidedName));
            return services;
        }

        public static IServiceCollection AddRabbitMQPublisher(this IServiceCollection services,IServiceProvider serviceProvider, string exchangeName, string exchangeType, TimeSpan? timeToLive,
           bool durable = true, bool autodelete = false)
        {
            services.AddScoped<IPublisher>(x => new Publisher(serviceProvider.GetRequiredService<IConnectionProvider>(),
                exchangeName,exchangeType,timeToLive,durable,autodelete));
            return services;
        }
        public static IServiceCollection AddRabbitMQSubscriber(this IServiceCollection services, 
            IServiceProvider serviceProvider, string exchange, 
            string exchangeType, string queue, string routingKey, 
            TimeSpan? timeToLive, ushort prefetchCount = 5,
            bool durable = true, bool exclusive = false, bool autodelete = false)
        {
            services.AddScoped<ISubscriber>(x => new Subscriber(serviceProvider.GetRequiredService<IConnectionProvider>(),
                exchange, exchangeType,queue,routingKey, timeToLive, prefetchCount,durable,exclusive,autodelete));
            return services;
        }
    }
}
