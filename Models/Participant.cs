namespace LeagueAPIConsumption.Models
{
    public class Participant
    {
        public long TeamId { get; set; }
        public long Spell1Id { get; set; }
        public long Spell2Id { get; set; }
        public long ChampionId { get; set; }
        public long ProfileIconId { get; set; }
        public bool Bot { get; set; }
        public string SummonerId { get; set; }
        public string SummonerName { get; set; }
        public GameCustomizationObject[] GameCustomizationObjects { get; set; }
        public Perks Perks { get; set; }
    }
}
