
using Limbus_wordle_backend.Interfaces;
using Limbus_wordle_backend.Models;

namespace Limbus_wordle_backend.Repository
{
    public class GameLobbyRepository : IGameLobbyRepository
    {
        private static readonly Dictionary<Guid, GameLobby> gameLobbies = new();

        public async Task<GameLobby> CreateGameLobby(GameLobby gameLobby)
        {
            gameLobbies.Add(gameLobby.Id, gameLobby);
            return await Task.FromResult(gameLobby);
        }

        public async Task DeleteGameLobby(Guid id)
        {
            gameLobbies.Remove(id);
            await Task.CompletedTask;
        }

        public async Task<List<GameLobby>> GetAllGameLobby()
        {
            return await Task.FromResult(gameLobbies.Values.ToList());
        }

        public async Task<GameLobby?> GetGameLobbyById(Guid id)
        {
            return await Task.FromResult(gameLobbies[id]);
        }

        public async Task<GameLobby> UpdateGameLobby(GameLobby gameLobby)
        {
            gameLobbies[gameLobby.Id] = gameLobby;
            await Task.CompletedTask;
            return gameLobby;
        }
    }
}