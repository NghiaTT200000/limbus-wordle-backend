namespace Limbus_wordle_backend.Models.DTO
{
    public class GuessResultDTO
    {
        public bool Correct { get; set; }
        public bool GameOver { get; set; }
        public int AttemptsRemaining { get; set; }
        public int NewScore { get; set; }
    }
}
