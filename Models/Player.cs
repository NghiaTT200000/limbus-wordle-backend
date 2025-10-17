namespace Limbus_wordle_backend.Models
{
    public class Player<GuessModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int Score { get; set; } = 0;
        public bool IsHost { get; set; } = false;
        public List<GuessModel> Guesses { get; set; } = new List<GuessModel>();
        public required GuessModel CurrentGuess { get; set; }
    }
}