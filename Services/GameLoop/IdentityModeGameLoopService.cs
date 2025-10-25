using System.Threading.Tasks;
using Limbus_wordle_backend.Interfaces;
using Limbus_wordle_backend.Models;
using Newtonsoft.Json;

namespace Limbus_wordle_backend.Services.GameLoop
{
    public class IdentityModeGameLoopService(int MaxGuess = 6) : IGameLoop
    {
        public int MaxGuess { get; set; } = MaxGuess;
        private IdentityFileService identityFileService = new IdentityFileService();

        public Player Guess(Player player, object guess)
        {
            if(!player.IsGameOver || player.Guesses.Count < MaxGuess)
            {
                var correctIdentityGuess = player.CurrentGuess as Identity;

                if (guess is Identity identityGuess)
                {
                    player.Guesses.Add(identityGuess);
                    if(JsonConvert.SerializeObject(identityGuess) == JsonConvert.SerializeObject(correctIdentityGuess))
                    {
                        player.Score += 1;
                        player.IsGameOver = true;
                    }
                    else if(player.Guesses.Count >= MaxGuess)
                    {
                        player.IsGameOver = true;
                    }
                }
            }
            return player;
        }

        public async Task<Player> ResetGameState(Player player)
        {
            player.IsGameOver = false;
            player.Guesses.Clear();
            player.CurrentGuess = await identityFileService.randomIdentity();
            return player;
        }

        public async Task<Player> StartGame(Player player)
        {
            var resetedPlayer = await ResetGameState(player);
            resetedPlayer.Score = 0;
            return resetedPlayer;
        }
    }
}