using Limbus_wordle_backend.Models.Entities;

namespace Limbus_wordle_backend.Models.DTOs
{
    public class PlayerProgressUpdateDTO
    {
        public int Score { get; set; } = 0;
        public bool IsReady { get; set; } = false;
        public int RoundWon { get; set; } = 0;
        public Identity CorrectGuess { get; set; }
        public List<GuessRecord> GuessHistory { get; set; } = [];
    }
}