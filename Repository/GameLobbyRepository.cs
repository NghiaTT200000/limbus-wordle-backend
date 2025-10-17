
using Limbus_wordle_backend.Interfaces;

namespace Limbus_wordle_backend.Repository
{
    public class GameLobbyRepository<GuessModel> : IGameLobbyRepository<GuessModel>
    {
        private static readonly Dictionary<Guid, Models.GameLobby<GuessModel>> gameLobbies = new ();

        public async Task<Models.GameLobby<GuessModel>> CreateGameLobby(Models.GameLobby<GuessModel> gameLobby)
        {
            gameLobbies.Add(gameLobby.Id, gameLobby);
            return await Task.FromResult(gameLobby);
        }

        public async Task DeleteGameLobby(Guid id)
        {
            gameLobbies.Remove(id);
            await Task.CompletedTask;
        }

        public async Task<List<Models.GameLobby<GuessModel>>> GetAllGameLobby()
        {
            return await Task.FromResult(gameLobbies.Values.ToList());
        }

        public async Task<Models.GameLobby<GuessModel>?> GetGameLobbyById(Guid id)
        {
            return await Task.FromResult(gameLobbies[id]);
        }

        public async Task UpdateGameLobby(Models.GameLobby<GuessModel> gameLobby)
        {
            gameLobbies[gameLobby.Id] = gameLobby;
            await Task.CompletedTask;
        }
    }
}