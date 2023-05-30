namespace AbstractedRabbitMQ.Subscribers
{
    public interface ISubscriber
    {
        Task Subscribe(Func<string, IDictionary<string, object>, bool> callBack);
        Task SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callBack);
        Task Subscribe<T>(Func<SubResult<T>, IDictionary<string, object>, bool> callBack);
        Task SubscribeAsync<T>(Func<SubResult<T>, IDictionary<string, object>, Task<bool>> callBack);
    }
}