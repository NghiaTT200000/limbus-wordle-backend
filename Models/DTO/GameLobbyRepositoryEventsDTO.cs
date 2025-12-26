namespace Limbus_wordle_backend.Models.DTO
{
    public class GameLobbyRepositoryEventsDTO
    {
        public Func<string, DateTime?, DateTime?, Task>? GameStarted { get; set; }
        public Func<string, int, Task>? TimeTick { get; set; }
        public Func<string, Task>? GameEnded { get; set; }
    }
}