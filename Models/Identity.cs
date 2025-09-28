using Limbus_wordle_backend.Interfaces;

namespace Limbus_wordle_backend.Models
{
    public class Identity : IEntity
    {
        public string Name { get; set; } = "";
        public string Sinner { get; set; } = "";
        public string Icon { get; set; } = "";
        public List<Skill> Skills { get; set; } = [];
    }
}