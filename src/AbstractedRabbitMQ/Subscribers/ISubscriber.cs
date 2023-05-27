namespace AbstractedRabbitMQ.Subscribers
{
    public interface ISubscriber:IDisposable
    {
        void Subscribe(Func<string, IDictionary<string, object>, bool> callBack);
    }
}