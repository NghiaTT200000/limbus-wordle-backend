using Limbus_wordle_backend.Models.DTO;

namespace Limbus_wordle_backend.Services
{
    public class GameLobbyService<GuessModel>
    {
        private readonly Repository.GameLobbyRepository<GuessModel> _gameLobbyRepository;

        public GameLobbyService(Repository.GameLobbyRepository<GuessModel> gameLobbyRepository)
        {
            _gameLobbyRepository = gameLobbyRepository;
        }

        public async Task<Models.GameLobby<GuessModel>> CreateGameLobby(Models.GameLobby<GuessModel> gameLobby)
        {
            return await _gameLobbyRepository.CreateGameLobby(gameLobby);
        }

        public async Task DeleteGameLobby(Guid id)
        {
            await _gameLobbyRepository.DeleteGameLobby(id);
        }

        public async Task<List<Models.GameLobby<GuessModel>>> GetAllGameLobby()
        {
            return await _gameLobbyRepository.GetAllGameLobby();
        }

        public async Task EnterPublicLobby(Guid lobbyId, PlayerSearchingLobbyDTO player)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(lobbyId);
        }
    }
}