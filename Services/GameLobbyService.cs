using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Models.DTO;
using Limbus_wordle_backend.Models.Exceptions;
using Limbus_wordle_backend.Repository;

namespace Limbus_wordle_backend.Services
{
    public class GameLobbyService(GameLobbyRepository gameLobbyRepository) : IDisposable
    {
        private readonly GameLobbyRepository _gameLobbyRepository = gameLobbyRepository;
        private GameLobbyRepositoryEventsDTO RepositoryEvents = new();

        public async Task<GameLobby> CreateGameLobby(GameLobby gameLobby)
        {
            return await _gameLobbyRepository.CreateGameLobby(gameLobby);
        }

        public async Task DeleteGameLobby(string lobbyCode)
        {
            await _gameLobbyRepository.DeleteGameLobby(lobbyCode);
        }

        public async Task<List<GameLobby>> GetAllGameLobby()
        {
            return await _gameLobbyRepository.GetAllGameLobby();
        }

        public async Task<List<GameLobby>> GetAllGameLobby(bool isPrivate)
        {
            var lobbies = await _gameLobbyRepository.GetAllGameLobby();
            return [.. lobbies.Where(l => l.IsPrivate == isPrivate)];
        }

        public async Task<Player?> RemovePlayerFromLobby(PlayerRemoveDTO playerRemoveDTO)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(playerRemoveDTO.LobbyCode) ??
                throw new LobbyNotFoundException();
            var player = lobby.Players.FirstOrDefault(p => p.Id == playerRemoveDTO.PlayerId);
            if (player != null)
            {
                lobby.Players.Remove(player);
            }
            if(playerRemoveDTO.IsHost)
            {
                if(lobby.Players.Count == 0)
                {
                    await _gameLobbyRepository.DeleteGameLobby(lobby.LobbyCode);
                    return player;
                }
                var newHost = lobby.Players.FirstOrDefault(p => p.Id == playerRemoveDTO.NewHostId);
                if(newHost != null)
                {
                    lobby.Players[lobby.Players.IndexOf(newHost)].IsHost = true;
                }
                else lobby.Players[0].IsHost = true;
            }
            await _gameLobbyRepository.UpdateGameLobby(lobby);
            return player;
        }

        public async Task<Player> MoveHost(string lobbyCode, Guid newHostId)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(lobbyCode) ??
                throw new LobbyNotFoundException();
            var currentHost = lobby.Players.FirstOrDefault(p => p.IsHost);
            if (currentHost != null)
            {
                currentHost.IsHost = false;
            }
            var newHost = lobby.Players.FirstOrDefault(p => p.Id == newHostId)
                ?? throw new PlayerNotFoundException();
            newHost.IsHost = true;
            await _gameLobbyRepository.UpdateGameLobby(lobby);
            return newHost;
        }

        public async Task<GameLobby?> AddPlayerToLobby(string lobbyCode, Player player)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(lobbyCode) ??
                throw new LobbyNotFoundException();
            if(!lobby.Players.Contains(player))
            {
                player.LobbyCode = lobbyCode;
                lobby.Players.Add(player);
            }
            return await _gameLobbyRepository.UpdateGameLobby(lobby);
        }

        public async Task<GameLobby> EnterPublicLobby(string lobbyCode, Player player)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(lobbyCode) ?? throw new LobbyNotFoundException();
            lobby.Players.Add(player);
            return await _gameLobbyRepository.UpdateGameLobby(lobby);
        }

        public async Task<GameLobby> GetGameLobbyById(string lobbyCode)
        {
            return await _gameLobbyRepository.GetGameLobbyById(lobbyCode) ?? throw new LobbyNotFoundException();
        }

        public async Task<List<Message>> SendMessageToLobby(string lobbyCode, Message message)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(lobbyCode) ?? throw new LobbyNotFoundException();
            lobby.Messages.Add(message);
            await _gameLobbyRepository.UpdateGameLobby(lobby);
            return lobby.Messages;
        }

        public async Task<GameLobby> UpdateGameLobby(GameLobbyUpdateDTO gameLobbyUpdateDTO, string lobbyCode)
        {
            var lobby = await _gameLobbyRepository.GetGameLobbyById(lobbyCode) ?? throw new LobbyNotFoundException();
            lobby.MaxPlayers = gameLobbyUpdateDTO.MaxPlayers;
            lobby.GameLength = gameLobbyUpdateDTO.GameLength;
            return await _gameLobbyRepository.UpdateGameLobby(lobby);
        }

        public async Task<Player> Guess(Player player, object guess)
        {
            return await _gameLobbyRepository.Guess(player, guess) ?? throw new PlayerNotFoundException();
        }


        public async Task StartGameAsync(string lobbyCode)
        {
            await _gameLobbyRepository.StartGameAsync(lobbyCode);
        }

        public async Task EndGameAsync(string lobbyCode, string reason)
        {
            await _gameLobbyRepository.EndGameAsync(lobbyCode, reason);
        }

        public void SubscribeToEvents(GameLobbyRepositoryEventsDTO eventsDTO)
        {
            RepositoryEvents = eventsDTO;
            if (RepositoryEvents.GameStarted != null)
            {
                _gameLobbyRepository.GameStarted += RepositoryEvents.GameStarted;
            }
            if (RepositoryEvents.TimeTick != null)
            {
                _gameLobbyRepository.TimeTick += RepositoryEvents.TimeTick;
            }
            if (RepositoryEvents.GameEnded != null)
            {
                _gameLobbyRepository.GameEnded += RepositoryEvents.GameEnded;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (RepositoryEvents.GameStarted != null)
            {
                _gameLobbyRepository.GameStarted -= RepositoryEvents.GameStarted;
            }
            if (RepositoryEvents.TimeTick != null)
            {
                _gameLobbyRepository.TimeTick -= RepositoryEvents.TimeTick;
            }
            if (RepositoryEvents.GameEnded != null)
            {
                _gameLobbyRepository.GameEnded -= RepositoryEvents.GameEnded;
            }
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }
    }
}