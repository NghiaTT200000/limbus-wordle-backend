namespace Limbus_wordle_backend.Models.DTO
{
    public class MessageDTO
    {
        public string Msg { get; set; } = "";
        public string UserName { get; set; } = "";
        public DateTime TimeStamp { get; set; }
        public bool IsSystem { get; set; }
    }
}
