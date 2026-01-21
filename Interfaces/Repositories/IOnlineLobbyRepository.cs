namespace Limbus_wordle_backend.Interfaces.Repositories
{
    using Limbus_wordle_backend.Models.DTOs;
    using Limbus_wordle_backend.Models.Entities;

    public interface IOnlineLobbyRepository
    {
        Task<OnlineLobby> CreateLobby(OnlineLobby lobby);
        Task<OnlineLobby?> GetLobbyById(string id);
        Task<OnlineLobby?> UpdateLobby(string id, OnlineLobbyUpdateDTO updateDTO);
        Task<OnlineLobby?> DeleteLobby(string id);
    }
}