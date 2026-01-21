using Limbus_wordle_backend.Interfaces.Repositories;
using StackExchange.Redis;

namespace Limbus_wordle_backend.Repositories
{
    public class BaseRepository: IBaseRepostiory
    {
        protected readonly IDatabase _database;
        protected readonly IConnectionMultiplexer _redis;

        public BaseRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
        }
    }
}