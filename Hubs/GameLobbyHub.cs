using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Services;
using Microsoft.AspNetCore.SignalR;

namespace Limbus_wordle_backend.Hubs
{
    public class GameLobbyHub(GameLobbyService gameLobbyService, AuthService authService) : Hub
    {
        private readonly GameLobbyService _gameLobbyService = gameLobbyService;
        private readonly AuthService _authService = authService;

        public override Task OnConnectedAsync()
        {
            // Get player info from auth service

            // If player is null,

            return base.OnConnectedAsync();
        }
    }
}