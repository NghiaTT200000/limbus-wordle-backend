namespace Limbus_wordle_backend.Models.Entities
{
    public class LobbyMessage
    {
        public Guid Id { get; set; }
        public Guid LobbyId { get; set; }
        public Guid PlayerId { get; set; }
        public string Message { get; set; } = "";
        public DateTime SentAt { get; set; }
    }
}