using System.IO.Abstractions;
using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Util.Environment;
using Limbus_wordle_backend.Util.Functions;
using Newtonsoft.Json;

namespace Limbus_wordle_backend.Services
{
    public class DailyIdentityFileService
    {
        private DailyIdentityFile DailyIdentityFile {get; set;}
        private IdentityFileService _identityFileService { get; set; }
        public DailyIdentityFileService(IdentityFileService identityFileService)
        {
            var defaultYesterdayIdentity = new Identity()
            {
                Name = "Full-Stop Office Rep Hong Lu",
                Sinner = "Hong Lu",
                Icon = "https://res.cloudinary.com/dpbeh6hnn/image/upload/v1736562718/Full-Stop Office Rep Hong Lu.webp",
                Skills = [
                    new ()
                    {
                        AttackType = "Pierce",
                        SinAffinity = "Sloth",
                        SkillCoinCount = 3
                    },
                    new ()
                    {
                        AttackType = "Slash",
                        SinAffinity = "Pride",
                        SkillCoinCount = 2
                    },
                    new ()
                    {
                        AttackType = "Pierce",
                        SinAffinity = "Gloom",
                        SkillCoinCount = 3
                    }
                ]
            };
            var defaultTodayIdentity = new Identity()
            {
                Name = "Heishou Pack - Si Branch Rodion",
                Sinner = "Rodion",
                Icon = "https://res.cloudinary.com/dpbeh6hnn/image/upload/v1747486486/Heishou Pack - Si Branch Rodion.webp",
                Skills = [
                    new ()
                    {
                        AttackType = "Slash",
                        SinAffinity = "Envy",
                        SkillCoinCount = 2
                    },
                    new ()
                    {
                        AttackType = "Slash",
                        SinAffinity = "Gluttony",
                        SkillCoinCount = 3
                    },
                    new ()
                    {
                        AttackType = "Pierce",
                        SinAffinity = "Gloom",
                        SkillCoinCount = 3
                    }
                ]
            };
            DailyIdentityFile = new DailyIdentityFile()
            {
                TodayID = Guid.NewGuid().ToString(),
                YesterdayIdentity = defaultYesterdayIdentity,
                TodayIdentity = defaultTodayIdentity
            };
            _identityFileService = identityFileService;
        }

        public DailyIdentityFile GetDailyIdentityFile()
        {
            return DailyIdentityFile;
        } 

        public async Task Reset()
        {
            var rootLink = Directory.GetCurrentDirectory();
            var dailyIdentityFilePath = Path.Combine(rootLink, EnvironmentVariables.dailyIdentityFilePath);
            var fileSystem = new FileSystem();
            try
            {
                string dailyIdentityFile = await fileSystem.File.ReadAllTextAsync(dailyIdentityFilePath);
                DailyIdentityFile deserializeDailyIdentities = new()
                {
                    TodayID = Guid.NewGuid().ToString(),
                    TodayIdentity = await _identityFileService.randomIdentity(),
                    YesterdayIdentity = await _identityFileService.randomIdentity(),
                };
                DailyIdentityFile? yesterdayIdentityFile = JsonConvert.DeserializeObject<DailyIdentityFile>(dailyIdentityFile);

                if(yesterdayIdentityFile!=null)
                {
                    deserializeDailyIdentities.TodayID = Guid.NewGuid().ToString();
                    deserializeDailyIdentities.YesterdayIdentity = yesterdayIdentityFile.TodayIdentity;
                    deserializeDailyIdentities.TodayIdentity = await _identityFileService.randomIdentity();
                } 

                Console.WriteLine("Daily: "+JsonConvert.SerializeObject(deserializeDailyIdentities));
                await File.WriteAllTextAsync(
                    Path.Combine(rootLink, EnvironmentVariables.dailyIdentityFilePath),
                    JsonConvert.SerializeObject(deserializeDailyIdentities,Formatting.Indented)
                );
                DailyIdentityFile = deserializeDailyIdentities;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}