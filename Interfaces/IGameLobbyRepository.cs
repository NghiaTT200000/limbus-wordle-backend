using Limbus_wordle_backend.Models;

namespace Limbus_wordle_backend.Interfaces
{
    public interface IGameLobbyRepository
    {
        Task<GameLobby> CreateGameLobby(GameLobby gameLobby);
        Task<List<GameLobby>> GetAllGameLobby();
        Task<GameLobby?> GetGameLobbyById(Guid id);
        Task<GameLobby> UpdateGameLobby(GameLobby gameLobby);
        Task DeleteGameLobby(Guid id);
    }
}