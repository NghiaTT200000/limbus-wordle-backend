namespace Limbus_wordle_backend.Models.Entities
{
    public class SkillMatch
    {
        public bool AttackTypeMatch { get; set; }
        public bool SinAffinityMatch { get; set; }
        public bool CoinCountMatch { get; set; } 
        public string AttackType { get; set; } = "";
        public string SinAffinity { get; set; } = "";
        public int SkillCoinCount { get; set; }
    }
}