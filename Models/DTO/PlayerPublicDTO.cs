namespace Limbus_wordle_backend.Models.DTO
{
    public class PlayerPublicDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int Score { get; set; }
        public bool IsHost { get; set; }
        public bool IsGameOver { get; set; }
    }
}
