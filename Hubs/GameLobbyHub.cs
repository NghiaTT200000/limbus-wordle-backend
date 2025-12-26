using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Models.DTO;
using Limbus_wordle_backend.Services;
using Limbus_wordle_backend.Util.Environment;
using Microsoft.AspNetCore.SignalR;

namespace Limbus_wordle_backend.Hubs
{
    public class GameLobbyHub(GameLobbyService gameLobbyService, AuthService authService) : Hub
    {
        private readonly GameLobbyService _gameLobbyService = gameLobbyService;
        private readonly AuthService _authService = authService;
        private static readonly Dictionary<string, Player> _connectionToPlayer = new();

        #region Helper Methods

        private Player? GetAuthenticatedPlayer()
        {
            if (Context.Items.TryGetValue(EnvironmentVariables.playerDataClaimName, out var obj) && obj is Player player)
            {
                return player;
            }
            return null;
        }

        private PlayerPublicDTO ToPublicPlayer(Player player)
        {
            return new PlayerPublicDTO
            {
                Id = player.Id,
                Name = player.Name,
                Score = player.Score,
                IsHost = player.IsHost,
                IsGameOver = player.IsGameOver
            };
        }

        private LobbyFullDTO ToFullLobby(GameLobby lobby)
        {
            return new LobbyFullDTO
            {
                LobbyCode = lobby.LobbyCode,
                IsPrivate = lobby.IsPrivate,
                MaxPlayers = lobby.MaxPlayers,
                GameLength = lobby.GameLength,
                Mode = lobby.Mode,
                IsGameStarted = lobby.IsGameStarted,
                RemainingTime = lobby.RemainingTimeUtc,
                Players = lobby.Players.Select(p => ToPublicPlayer(p)).ToList(),
                Messages = lobby.Messages.Select(m => new MessageDTO
                {
                    Msg = m.Msg,
                    UserName = m.UserName,
                    TimeStamp = m.TimeStamp,
                    IsSystem = m.IsSystem
                }).ToList()
            };
        }

        private PublicLobbyDTO ToPublicLobby(GameLobby lobby)
        {
            return new PublicLobbyDTO
            {
                LobbyCode = lobby.LobbyCode,
                CurrentPlayers = lobby.Players.Count,
                MaxPlayers = lobby.MaxPlayers,
                GameLength = lobby.GameLength,
                Mode = lobby.Mode,
                IsGameStarted = lobby.IsGameStarted,
                HostName = lobby.Players.FirstOrDefault(p => p.IsHost)?.Name ?? ""
            };
        }

        #endregion

        #region Connection Lifecycle

        public override async Task OnConnectedAsync()
        {
            var player = GetAuthenticatedPlayer();
            if (player != null)
            {
                _connectionToPlayer[Context.ConnectionId] = player;

                // If player was in a lobby, rejoin the SignalR group
                if (!string.IsNullOrEmpty(player.LobbyCode))
                {
                    try
                    {
                        var lobby = await _gameLobbyService.GetGameLobbyById(player.LobbyCode);
                        if (lobby != null && lobby.Players.Any(p => p.Id == player.Id))
                        {
                            await Groups.AddToGroupAsync(Context.ConnectionId, $"lobby_{player.LobbyCode}");
                        }
                    }
                    catch
                    {
                        // Lobby may have been deleted
                        player.LobbyCode = "";
                    }
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var player = GetAuthenticatedPlayer();
            if (player != null)
            {
                _connectionToPlayer.Remove(Context.ConnectionId);

                if (!string.IsNullOrEmpty(player.LobbyCode))
                {
                    await LeaveLobbyInternal();
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region Lobby Discovery

        public async Task<List<PublicLobbyDTO>> GetPublicLobbies()
        {
            var lobbies = await _gameLobbyService.GetAllGameLobby(isPrivate: false);
            return lobbies.Select(ToPublicLobby).ToList();
        }

        public async Task<JoinResultDTO> JoinLobbyByCode(string lobbyCode)
        {
            var player = GetAuthenticatedPlayer();
            if (player == null) return new JoinResultDTO { Success = false, Message = "Not authenticated" };

            try
            {
                var lobby = await _gameLobbyService.GetGameLobbyById(lobbyCode);
                if (lobby == null)
                    return new JoinResultDTO { Success = false, Message = "Invalid lobby code" };
                if (lobby.IsGameStarted)
                    return new JoinResultDTO { Success = false, Message = "Game already started" };
                if (lobby.Players.Count >= lobby.MaxPlayers)
                    return new JoinResultDTO { Success = false, Message = "Lobby full" };

                await _gameLobbyService.AddPlayerToLobby(lobbyCode, player);
                await Groups.AddToGroupAsync(Context.ConnectionId, $"lobby_{lobbyCode}");

                // Broadcast to others
                await Clients.OthersInGroup($"lobby_{lobbyCode}").SendAsync("PlayerJoined", ToPublicPlayer(player));

                lobby = await _gameLobbyService.GetGameLobbyById(lobbyCode);
                return new JoinResultDTO { Success = true, Lobby = ToFullLobby(lobby) };
            }
            catch (Exception ex)
            {
                return new JoinResultDTO { Success = false, Message = ex.Message };
            }
        }

        public async Task<JoinResultDTO> QuickMatch()
        {
            var player = GetAuthenticatedPlayer();
            if (player == null) return new JoinResultDTO { Success = false, Message = "Not authenticated" };

            // Find available public lobby
            var publicLobbies = await _gameLobbyService.GetAllGameLobby(isPrivate: false);
            var availableLobby = publicLobbies
                .Where(l => !l.IsGameStarted && l.Players.Count < l.MaxPlayers)
                .OrderByDescending(l => l.Players.Count)
                .FirstOrDefault();

            if (availableLobby != null)
            {
                return await JoinLobbyByCode(availableLobby.LobbyCode);
            }

            // Create new lobby with default settings
            return await CreateLobby(new GameLobbyCreateDTO
            {
                MaxPlayers = 6,
                IsPrivate = false,
                GameLength = TimeSpan.FromMinutes(2),
                Mode = GameMode.IdentityMode
            });
        }

        public async Task<JoinResultDTO> CreateLobby(GameLobbyCreateDTO createDTO)
        {
            var player = GetAuthenticatedPlayer();
            if (player == null) return new JoinResultDTO { Success = false, Message = "Not authenticated" };

            try
            {
                var lobby = new GameLobby(createDTO);
                player.IsHost = true;
                await _gameLobbyService.CreateGameLobby(lobby);
                await _gameLobbyService.AddPlayerToLobby(lobby.LobbyCode, player);
                await Groups.AddToGroupAsync(Context.ConnectionId, $"lobby_{lobby.LobbyCode}");

                return new JoinResultDTO { Success = true, Lobby = ToFullLobby(lobby) };
            }
            catch (Exception ex)
            {
                return new JoinResultDTO { Success = false, Message = ex.Message };
            }
        }

        #endregion

        #region In-Lobby Methods

        public async Task<bool> LeaveLobby()
        {
            var player = GetAuthenticatedPlayer();
            if (player == null || string.IsNullOrEmpty(player.LobbyCode)) return false;

            await LeaveLobbyInternal();
            return true;
        }

        private async Task LeaveLobbyInternal()
        {
            var player = GetAuthenticatedPlayer();
            if (player == null || string.IsNullOrEmpty(player.LobbyCode)) return;

            var lobbyCode = player.LobbyCode;
            var lobby = await _gameLobbyService.GetGameLobbyById(lobbyCode);
            if (lobby == null) return;

            var playerInLobby = lobby.Players.FirstOrDefault(p => p.Id == player.Id);
            if (playerInLobby == null) return;

            var isHost = playerInLobby.IsHost;

            var removeDTO = new PlayerRemoveDTO
            {
                PlayerId = player.Id,
                LobbyCode = lobbyCode,
                IsHost = isHost,
                NewHostId = isHost ? lobby.Players.FirstOrDefault(p => p.Id != player.Id)?.Id ?? Guid.Empty : Guid.Empty
            };

            await _gameLobbyService.RemovePlayerFromLobby(removeDTO);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"lobby_{lobbyCode}");

            await Clients.Group($"lobby_{lobbyCode}").SendAsync("PlayerLeft", player.Id);
            if (isHost && removeDTO.NewHostId != Guid.Empty)
            {
                await Clients.Group($"lobby_{lobbyCode}").SendAsync("HostMigrated", removeDTO.NewHostId);
            }

            player.LobbyCode = "";
            player.IsHost = false;
        }

        public async Task<bool> StartGame()
        {
            var player = GetAuthenticatedPlayer();
            if (player == null || string.IsNullOrEmpty(player.LobbyCode)) return false;

            var lobby = await _gameLobbyService.GetGameLobbyById(player.LobbyCode);
            if (lobby == null) return false;

            // Only host can start
            var requester = lobby.Players.FirstOrDefault(p => p.Id == player.Id);
            if (requester == null || !requester.IsHost) return false;

            await _gameLobbyService.StartGameAsync(player.LobbyCode);

            lobby = await _gameLobbyService.GetGameLobbyById(player.LobbyCode);
            await Clients.Group($"lobby_{player.LobbyCode}").SendAsync("GameStarted", new
            {
                StartTimeUtc = lobby.StartTimeUtc,
                EndTimeUtc = lobby.EndTimeUtc,
                GameLength = lobby.GameLength
            });

            return true;
        }

        public async Task<GuessResultDTO> SubmitGuess(object guess)
        {
            var player = GetAuthenticatedPlayer();
            if (player == null) return new GuessResultDTO { Correct = false, GameOver = true };

            var lobbyCode = player.LobbyCode;
            var lobby = await _gameLobbyService.GetGameLobbyById(lobbyCode);
            if (lobby == null) return new GuessResultDTO { Correct = false, GameOver = true };

            var oldScore = player.Score;
            var updatedPlayer = await _gameLobbyService.Guess(player, guess);

            // Broadcast score update to others (score-only, no guesses)
            if (updatedPlayer.Score != oldScore)
            {
                await Clients.OthersInGroup($"lobby_{lobbyCode}").SendAsync("ScoreUpdate", new
                {
                    PlayerId = player.Id,
                    NewScore = updatedPlayer.Score,
                    IsGameOver = updatedPlayer.IsGameOver
                });
            }

            return new GuessResultDTO
            {
                Correct = updatedPlayer.Score > oldScore,
                GameOver = updatedPlayer.IsGameOver,
                AttemptsRemaining = 6 - updatedPlayer.Guesses.Count,
                NewScore = updatedPlayer.Score
            };
        }

        #endregion
    }
}
