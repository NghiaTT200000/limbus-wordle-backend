namespace Limbus_wordle_backend.Models
{
    public class DailyIdentityFile
    {
        public string TodayID { get; set; } = "";
        public required Identity TodayIdentity { get; set; }
        public required Identity YesterdayIdentity { get; set; }
    }
}