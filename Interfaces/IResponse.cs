namespace Limbus_wordle_backend.Interfaces
{
    public interface IResponse<ResponseType>
    {
        ResponseType? Response { get; set;}
        string Msg { get; set; }
    }
}