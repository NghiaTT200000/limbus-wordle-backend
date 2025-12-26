
using System.Collections.Concurrent;
using Limbus_wordle_backend.Interfaces;
using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Models.Exceptions;
using Limbus_wordle_backend.Services.GameLoop;

namespace Limbus_wordle_backend.Repository
{
    public class GameLobbyRepository : IGameLobbyRepository
    {
        private static readonly ConcurrentDictionary<string, LobbyState> _gameLobbies = new();

        public event Func<string, DateTime?, DateTime?, Task>? GameStarted;
        public event Func<string, int, Task>? TimeTick;
        public event Func<string, Task>? GameEnded;

        public async Task<GameLobby> CreateGameLobby(GameLobby gameLobby)
        {
            var state = new LobbyState(gameLobby);
            _gameLobbies.TryAdd(gameLobby.LobbyCode, state);
            return await Task.FromResult(gameLobby);
        }

        public async Task DeleteGameLobby(string lobbyCode)
        {
            _gameLobbies.TryRemove(lobbyCode, out _);
            await Task.CompletedTask;
        }

        public async Task<List<GameLobby>> GetAllGameLobby()
        {
            return await Task.FromResult(_gameLobbies.Values.Select(ls => ls.Lobby).ToList());
        }

        public async Task<GameLobby?> GetGameLobbyById(string lobbyCode)
        {
            _gameLobbies.TryGetValue(lobbyCode, out var lobby);
            return await Task.FromResult(lobby?.Lobby);
        }

        public async Task<GameLobby> UpdateGameLobby(GameLobby gameLobby)
        {
            _gameLobbies.AddOrUpdate(gameLobby.LobbyCode, new LobbyState(gameLobby), (_, lobbyState) =>
                {
                    lobbyState.Lobby = gameLobby;
                    return lobbyState;
                });
            await Task.CompletedTask;
            return gameLobby;
        }

        public Player UpdatePlayer(Player player){
            var lobby = _gameLobbies[player.LobbyCode].Lobby;
            var updatedPlayer = lobby.Players.Find(p => p.Id == player.Id);
            updatedPlayer = player;

            return updatedPlayer;
        }

        public async Task StartGameAsync(string lobbyCode)
        {
            if(!_gameLobbies.TryGetValue(lobbyCode, out var lobbyState))
            {
                throw new LobbyNotFoundException();
            }
            await lobbyState.Semaphore.WaitAsync();
            try
            {
                if (lobbyState.Lobby.IsGameStarted) return;
                var gameLoop = lobbyState.Lobby.GameLoop;
                lobbyState.Lobby.IsGameStarted = true;
                lobbyState.Lobby.StartTimeUtc = DateTime.UtcNow;
                lobbyState.Lobby.EndTimeUtc = lobbyState.Lobby.StartTimeUtc.Value + lobbyState.Lobby.GameLength;

                var startTasks = lobbyState.Lobby.Players.Select(p => gameLoop.StartGame(p)).ToList();
                var startedPlayers = await Task.WhenAll(startTasks);
                lobbyState.Lobby.Players = startedPlayers.ToList();

                lobbyState.Cts = new CancellationTokenSource();
                lobbyState.RunTask = RunLobbyLoop(lobbyState, lobbyState.Cts.Token);
                GameStarted?.Invoke(lobbyState.Lobby.LobbyCode, lobbyState.Lobby.StartTimeUtc, lobbyState.Lobby.EndTimeUtc);
            }
            finally
            {
                lobbyState.Semaphore.Release();
            }
        }

        public async Task<Player?> Guess(Player player, object guess)
        {
            var lobby = await GetGameLobbyById(player.LobbyCode) ?? throw new LobbyNotFoundException();

            var updatedPlayer = lobby.GameLoop.Guess(player, guess);
            UpdatePlayer(updatedPlayer);
            return updatedPlayer;
        }

        public async Task EndGameAsync(string lobbyCode, string reason)
        {
            if (!_gameLobbies.TryGetValue(lobbyCode, out var gameState)) return;
            CancellationTokenSource? ctsToCancel = null;

            await gameState.Semaphore.WaitAsync();
            try
            {
                if (!gameState.Lobby.IsGameStarted) return;
                gameState.Lobby.IsGameStarted = false;
                gameState.Lobby.StartTimeUtc = null;
                gameState.Lobby.EndTimeUtc = null;
                ctsToCancel = gameState.Cts;
                gameState.Cts = null;
            }
            finally
            {
                gameState.Semaphore.Release();
            }

            ctsToCancel?.Cancel();
            GameEnded?.Invoke("Game Ended: " + reason);
        }

        private async Task RunLobbyLoop(LobbyState state, CancellationToken token)
        {
            var lobby = state.Lobby;
            var lobbyCode = lobby.LobbyCode;

            try
            {
                // initial tick immediately
                while (!token.IsCancellationRequested)
                {
                    var remaining = lobby.EndTimeUtc!.Value - DateTime.UtcNow;
                    var remainingSeconds = (int)Math.Ceiling(remaining.TotalSeconds);

                    if (remainingSeconds <= 0)
                    {
                        await (TimeTick?.Invoke(lobbyCode, 0) ?? Task.CompletedTask);
                        await EndGameAsync(lobbyCode, "time_up");
                        break;
                    }

                    await (TimeTick?.Invoke(lobbyCode, remainingSeconds) ?? Task.CompletedTask);

                    // use Task.Delay for tick; compute next delay to reduce drift
                    var delay = remaining > TimeSpan.FromSeconds(1) ? TimeSpan.FromSeconds(1) : remaining;
                    await Task.Delay(delay, token);
                }
            }
            catch (OperationCanceledException) {}
            catch (Exception)
            {
                // optionally raise GameEnded with error reason or log
                _ = GameEnded?.Invoke("error");
            }
        }


        private class LobbyState(GameLobby l)
        {
            public GameLobby Lobby { get; set; } = l; 
            public CancellationTokenSource? Cts { get; set; }
            public SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1,1);
            public Task? RunTask { get; set; }
        }
    }
}