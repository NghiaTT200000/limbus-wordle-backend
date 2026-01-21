namespace Limbus_wordle_backend.Models.Entities
{
    public class LobbyMessage
    {
        public string Id { get; set; } = "";
        public string LobbyId { get; set; } = "";
        public string PlayerId { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime SentAt { get; set; }
    }
}