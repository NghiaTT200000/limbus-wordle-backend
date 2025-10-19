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
        Task StartGameAsync(Guid lobbyId);
        Task EndGameAsync(Guid lobbyId, string reason);

        event Func<Guid, DateTime?, DateTime?, Task>? GameStarted;
        event Func<Guid, int, Task>? TimeTick;
        event Func<string, Task>? GameEnded;
    }
}