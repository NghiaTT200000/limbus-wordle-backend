using Limbus_wordle_backend.Models;

namespace Limbus_wordle_backend.Interfaces
{
    public interface IGameLobbyRepository<GuessModel>
    {
        Task<GameLobby<GuessModel>> CreateGameLobby(GameLobby<GuessModel> gameLobby);
        Task<List<GameLobby<GuessModel>>> GetAllGameLobby();
        Task<GameLobby<GuessModel>?> GetGameLobbyById(Guid id);
        Task UpdateGameLobby(GameLobby<GuessModel> gameLobby);
        Task DeleteGameLobby(Guid id);
    }
}