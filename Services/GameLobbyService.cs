using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Repository;

namespace Limbus_wordle_backend.Services
{
    public class GameLobbyService(GameLobbyRepository gameLobbyRepository)
    {
        private readonly GameLobbyRepository _gameLobbyRepository = gameLobbyRepository;

        public async Task<GameLobby> CreateGameLobby(GameLobby gameLobby)
        {
            return await _gameLobbyRepository.CreateGameLobby(gameLobby);
        }

        public async Task DeleteGameLobby(Guid id)
        {
            await _gameLobbyRepository.DeleteGameLobby(id);
        }

        public async Task<List<GameLobby>> GetAllGameLobby()
        {
            return await _gameLobbyRepository.GetAllGameLobby();
        }

        public async Task<GameLobby> EnterPublicLobby(Guid lobbyId, Player player)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(lobbyId);
            if (lobby == null)
            {
                throw new Exception("Lobby not found");
            }
            lobby.Players.Add(player);
            return await _gameLobbyRepository.UpdateGameLobby(lobby);
        }
    }
}