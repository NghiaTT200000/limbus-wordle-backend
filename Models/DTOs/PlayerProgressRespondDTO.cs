using Limbus_wordle_backend.Models.Entities;

namespace Limbus_wordle_backend.Models.DTOs
{
    public class PlayerProgressRespondDTO
    {
        public Guid Id { get; set; }
        public Guid LobbyId { get; set; }
        public string Name { get; set; } = "";
        public int Score { get; set; } = 0;
        public bool IsReady { get; set; } = false;
        public int RoundWon { get; set; } = 0;
        public List<GuessRecord> GuessHistory { get; set; } = [];
    }
}