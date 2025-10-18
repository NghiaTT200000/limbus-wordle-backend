namespace Limbus_wordle_backend.Models.Exceptions
{
    public class NoPlayerDataException : BaseException
    {
        public NoPlayerDataException() : base("No player data found in token", System.Net.HttpStatusCode.Unauthorized)
        {
        }
    }
}