using Limbus_wordle_backend.Models.DTO;

namespace Limbus_wordle_backend.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string LobbyCode { get; set; } = "";
        public string Name { get; set; } = "";
        public int Score { get; set; } = 0;
        public bool IsHost { get; set; } = false;
        public bool IsGameOver { get; set; } = true;
        public List<object> Guesses { get; set; } = [];
        public object CurrentGuess { get; set; } = new object();

        public Player(PlayerCreateDTO newPlayer){
            Name = newPlayer.Name;
            Id = Guid.NewGuid();
        }
    }
}