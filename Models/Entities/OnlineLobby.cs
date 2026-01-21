namespace Limbus_wordle_backend.Models.Entities
{
    public class OnlineLobby
    {
        public string Id { get; set; } = "";
        public string LobbyName { get; set;} = "";
        public bool IsPrivate { get; set; }
        public string HostPlayerId { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastIdle { get; set; }
        public LobbyStatus Status { get; set; } = LobbyStatus.IDLE;
        public enum LobbyStatus
        {
            IDLE,
            INPROGRESS,
        }
    }
}