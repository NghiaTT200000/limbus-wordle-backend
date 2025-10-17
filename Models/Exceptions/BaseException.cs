using System.Net;

namespace Limbus_wordle_backend.Models.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode statusCode { get; set; }

        public BaseException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.statusCode = statusCode;
        }
    }
}