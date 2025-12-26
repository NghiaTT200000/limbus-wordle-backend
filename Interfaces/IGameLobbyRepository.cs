using Limbus_wordle_backend.Models;

namespace Limbus_wordle_backend.Interfaces
{
    public interface IGameLobbyRepository
    {
        Task<GameLobby> CreateGameLobby(GameLobby gameLobby);
        Task<List<GameLobby>> GetAllGameLobby();
        Task<GameLobby?> GetGameLobbyById(string lobbyCode);
        Task<GameLobby> UpdateGameLobby(GameLobby gameLobby);
        Task DeleteGameLobby(string lobbyCode);
        Task StartGameAsync(string lobbyCode);
        Task EndGameAsync(string lobbyCode, string reason);

        event Func<string, DateTime?, DateTime?, Task>? GameStarted;
        event Func<string, int, Task>? TimeTick;
        event Func<string, Task>? GameEnded;
    }
}