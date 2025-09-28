using Limbus_wordle_backend.Interfaces;

namespace Limbus_wordle_backend.Models
{
    public class ResponseService<ResponseType> : IResponse<ResponseType>
    {
        public ResponseType? Response { get ; set ; }
        public string Msg { get ; set ; } = "";
    }
}