namespace Limbus_wordle_backend.Models.Entities
{
    public class GuessRecord
    {
        public int Id { get; set; }
        public Guid PlayerLobbyId { get; set; }
        public DateTime GuessedAt { get; set; }
        public string IdentityName { get; set; } = "";
        public string IdentityIcon { get; set; } = "";
        public string Sinner { get; set; } = "";
        public bool IsCorrectSinner { get; set; } = false;
        public bool IsCorrect { get; set; } = false;
        public List<SkillMatch> SkillMatches { get; set; } = [];

        public static GuessRecord ToGuessRecord(Identity identity, Identity correctIdentity)
        {
            return new GuessRecord
            {
                IdentityName = identity.Name,
                IdentityIcon = identity.Icon,
                Sinner = identity.Sinner,
                IsCorrectSinner = identity.Sinner.Equals(correctIdentity.Sinner),
                IsCorrect = identity.Name.Equals(correctIdentity.Name),
                SkillMatches = identity.Skills.Select((skill, index)=>
                {
                    var match = new SkillMatch()
                    {
                        AttackType = skill.AttackType,
                        SinAffinity = skill.SinAffinity,
                        SkillCoinCount = skill.SkillCoinCount,
                        AttackTypeMatch = skill.AttackType.Equals(correctIdentity.Skills[index].AttackType),
                        SinAffinityMatch = skill.SinAffinity.Equals(correctIdentity.Skills[index].SinAffinity),
                        CoinCountMatch = skill.SkillCoinCount == correctIdentity.Skills[index].SkillCoinCount
                    };
                    return match;
                }).ToList()
            };
        }
    }
}