namespace Limbus_wordle_backend.Models.DTO
{
    public class GameLobbyRepositoryEventsDTO
    {
        public Func<Guid, DateTime?, DateTime?, Task>? GameStarted { get; set; }
        public Func<Guid, int, Task>? TimeTick { get; set; }
        public Func<string, Task>? GameEnded { get; set; }
    }
}