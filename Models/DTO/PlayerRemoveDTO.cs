namespace Limbus_wordle_backend.Models.DTO
{
    public class PlayerRemoveDTO
    {
        public Guid PlayerId { get; set; }
        public string LobbyCode { get; set; } = "";
        public bool IsHost { get; set; }
        public Guid NewHostId { get; set; }
    }
}