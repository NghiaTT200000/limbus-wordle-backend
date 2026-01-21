using Limbus_wordle_backend.Models.Entities;

namespace Limbus_wordle_backend.Models.DTOs
{
    public class OnlineLobbyUpdateDTO
    {
        public string HostPlayerId { get; set; } = "";
        public OnlineLobby.LobbyStatus Status { get; set; }
    }
}