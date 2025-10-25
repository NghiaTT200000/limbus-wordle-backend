using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Services;
using Limbus_wordle_backend.Util.Environment;
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
            // If player is not null, check if player is in a room and connect back to the room
            if (Context.Items.TryGetValue(EnvironmentVariables.playerDataClaimName, out var obj) && obj is Player player)
            {
                _gameLobbyService.AddOrUpdatePlayer(Context.ConnectionId, player);
            }
            // If player is null,

            return base.OnConnectedAsync();
        }
    }
}