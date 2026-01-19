using Limbus_wordle_backend.Models.Entities;
using Limbus_wordle_backend.Util.Environment;
using Newtonsoft.Json;

namespace Limbus_wordle_backend.Repositories
{
    public class IdentityFileRepository
    {
        private readonly string identitiesFilePath;

        public IdentityFileRepository()
        {
            var rootLink = Directory.GetCurrentDirectory();
            identitiesFilePath = Path.Combine(rootLink, EnvironmentVariables.identitiesFilePath);
        }


        public async Task<Dictionary<string, Identity>> GetAllIdentities()
        {
            var identitiesJson = await File.ReadAllTextAsync(EnvironmentVariables.identitiesFilePath);
            var identities = JsonConvert.DeserializeObject<Dictionary<string, Identity>>(identitiesJson);
            if (identities == null) return new Dictionary<string, Identity>();
            return identities;
        }

        public async Task<Dictionary<string, Identity>> SaveAllIdentities(Dictionary<string, Identity> identities)
        {
            var identitiesJson = JsonConvert.SerializeObject(identities, Formatting.Indented);
            await File.WriteAllTextAsync(identitiesFilePath, identitiesJson);
            return identities;
        }

        public async Task<Identity> AddIdentity(string urlId,Identity identity)
        {
            var identities = await GetAllIdentities();
            identities[urlId] = identity;
            var identitiesJson = JsonConvert.SerializeObject(identities, Formatting.Indented);
            await File.WriteAllTextAsync(identitiesFilePath, identitiesJson);
            return identity;
        }

        public async Task<Identity> RandomIdentity()
        {
            var identities = await GetAllIdentities();
            Random random = new();
            return identities.ElementAt(random.Next(identities.Count)).Value;
        }
    }
}