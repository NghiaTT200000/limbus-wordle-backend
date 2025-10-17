using System.Net;

namespace Limbus_wordle_backend.Models.Exceptions
{
    public class DefaultException : BaseException
    {
        public DefaultException() : base("Something went wrong", HttpStatusCode.BadRequest)
        {
        }
    }
}