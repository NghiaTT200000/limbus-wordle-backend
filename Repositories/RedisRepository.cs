using StackExchange.Redis;

namespace Limbus_wordle_backend.Repositories
{
    public class RedisRepository(IConnectionMultiplexer redis) : BaseRepository(redis)
    {
        protected static string LobbyKey(string id) => $"limbus-wordle:lobby:{id}";
        protected static string PlayerKey(string id) => $"limbus-wordle:player:{id}";
        protected static string LobbyPlayersKey(string lobbyId) => $"limbus-wordle:lobby:{lobbyId}:players";
        protected static string LobbyMessagesKey(string lobbyId) => $"limbus-wordle:lobby:{lobbyId}:messages";
        protected static string CodeKey(string code) => $"limbus-wordle:code:{code}";
        protected static string PublicIndexKey() => $"limbus-wordle:index:public";
    }
}