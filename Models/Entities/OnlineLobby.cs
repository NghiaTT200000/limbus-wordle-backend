namespace Limbus_wordle_backend.Models.Entities
{
    public class OnlineLobby
    {
        public Guid Id { get; set; }
        public string LobbyCode { get; set; } = "";
        public string LobbyName { get; set;} = "";
        public bool IsPrivate { get; set; }
        public Guid HostPlayerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastIdle { get; set; }
        public List<PlayerProgress> Players { get; set; } = [];
        public List<LobbyMessage> Messages { get; set; } = [];
        public LobbyStatus Status { get; set; } = LobbyStatus.IDLE;
        public enum LobbyStatus
        {
            IDLE,
            INPROGRESS,
        }
    }
}