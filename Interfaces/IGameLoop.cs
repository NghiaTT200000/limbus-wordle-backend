using Limbus_wordle_backend.Models;

namespace Limbus_wordle_backend.Interfaces
{
    public interface IGameLoop
    {
        int MaxGuess { get; set; }
        Player Guess (Player player, object guess);
        Task<Player> StartGame(Player player);
        Task<Player> ResetGameState(Player player);
    }
}