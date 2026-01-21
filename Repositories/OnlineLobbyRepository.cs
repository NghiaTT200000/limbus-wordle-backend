using System.Text.Json;
using System.Threading.Tasks;
using Limbus_wordle_backend.Interfaces.Repositories;
using Limbus_wordle_backend.Models.DTOs;
using Limbus_wordle_backend.Models.Entities;
using StackExchange.Redis;

namespace Limbus_wordle_backend.Repositories
{
    public class OnlineLobbyRepository(IConnectionMultiplexer redis) : RedisRepository(redis), IOnlineLobbyRepository
    {
        public async Task<OnlineLobby> CreateLobby(OnlineLobby lobby)
        {
            var lobbyKey = LobbyKey(lobby.Id);
            var lobbyData = JsonSerializer.Serialize(lobby);
            var expiry = TimeSpan.FromHours(24);

            await _database.StringSetAsync(lobbyKey, lobbyData, expiry);

            if(!lobby.IsPrivate)
            {
                var score = lobby.CreatedAt.Ticks;
                await _database.SortedSetAddAsync(PublicIndexKey(), lobby.Id, score);
            }

            return lobby;
        }

        public async Task<OnlineLobby?> DeleteLobby(string id)
        {
            var lobby = await GetLobbyById(id);
            if(lobby != null)
            {
                var lobbyKey = LobbyKey(id);
                await _database.KeyDeleteAsync(lobbyKey);

                if(!lobby.IsPrivate)
                {
                    await _database.SortedSetRemoveAsync(PublicIndexKey(), id);
                }

            }
            
            return lobby;
        }

        public async Task<OnlineLobby?> GetLobbyById(string id)
        {
            var lobby = await _database.StringGetAsync(LobbyKey(id));
            return lobby.IsNullOrEmpty ? null : JsonSerializer.Deserialize<OnlineLobby>(lobby!);
        }

        public async Task<OnlineLobby?> UpdateLobby(string id, OnlineLobbyUpdateDTO updateDTO)
        {
            var lobby = await GetLobbyById(id);
            if(lobby != null)
            {
                lobby.HostPlayerId  = updateDTO.HostPlayerId;
                lobby.Status = updateDTO.Status;
                lobby.UpdatedAt = DateTime.UtcNow;

                var lobbyKey = LobbyKey(id);
                await _database.StringSetAsync(lobbyKey, JsonSerializer.Serialize(lobby));
            }
            return lobby;
        }
    }
}