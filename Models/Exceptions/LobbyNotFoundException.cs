using System.Net;

namespace Limbus_wordle_backend.Models.Exceptions
{
    public class LobbyNotFoundException : BaseException
    {
        public LobbyNotFoundException() : base("Lobby not found", HttpStatusCode.NotFound)
        {
        }
    }
}