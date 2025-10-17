namespace Limbus_wordle_backend.Models
{
    public class Message
    {
        public string Msg { get; set; } = "";
        public string UserName { get; set; } = "";
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public bool IsSystem { get; set; } = false;
    }
}