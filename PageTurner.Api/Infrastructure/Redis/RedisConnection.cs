using StackExchange.Redis;

namespace PageTurner.Api.Infrastructure.Redis
{
    public class RedisConnection
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisConnection(string connectionString)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(connectionString)
            );
        }

        public IDatabase GetDatabase()
        {
            return _lazyConnection.Value.GetDatabase();
        }
    }
}
