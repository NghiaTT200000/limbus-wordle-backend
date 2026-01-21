namespace Limbus_wordle_backend.Models.DTOs
{
    public class PlayerProgressCreateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
    }
}