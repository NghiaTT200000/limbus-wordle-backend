namespace Limbus_wordle_backend.Models.Exceptions
{
    public class PlayerNotFoundException : BaseException
    {
        public PlayerNotFoundException() : base("Player not found", System.Net.HttpStatusCode.NotFound)
        {
        }
    }
}