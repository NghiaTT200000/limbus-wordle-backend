using System.IO.Abstractions;
using System.Threading.Tasks;
using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Util.Environment;
using Newtonsoft.Json;

namespace Limbus_wordle_backend.Services
{
    public class IdentityFileService
    {
        private string identitiesFilePath;

        public IdentityFileService()
        {
            var rootLink = Directory.GetCurrentDirectory();
            identitiesFilePath = Path.Combine(rootLink, EnvironmentVariables.identitiesFilePath);
        }


        public async Task<Dictionary<string, Identity>> getAllIdentities()
        {
            var identitiesJson = await File.ReadAllTextAsync(EnvironmentVariables.identitiesFilePath);
            var identities = JsonConvert.DeserializeObject<Dictionary<string, Identity>>(identitiesJson);
            if (identities == null) return new Dictionary<string, Identity>();
            return identities;
        }

        public async Task<Dictionary<string, Identity>> saveAllIdentities(Dictionary<string, Identity> identities)
        {
            var identitiesJson = JsonConvert.SerializeObject(identities, Formatting.Indented);
            await File.WriteAllTextAsync(identitiesFilePath, identitiesJson);
            return identities;
        }

        public async Task<Identity> addIdentitty(string urlId,Identity identity)
        {
            var identities = await getAllIdentities();
            identities[urlId] = identity;
            var identitiesJson = JsonConvert.SerializeObject(identities, Formatting.Indented);
            await File.WriteAllTextAsync(identitiesFilePath, identitiesJson);
            return identity;
        }

        public async Task<Identity> randomIdentity()
        {
            var identities = await getAllIdentities();
            Random random = new Random();
            return identities.ElementAt(random.Next(identities.Count)).Value;
        }
    }
}