using LeagueAPIConsumption.DTO.LiveMatch;

namespace LeagueAPIConsumption.Models
{
    public class CurrentGameParticipant
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

        public bool win { get; set; }

        public int playerScore0 { get; set; }
        public int playerScore1 { get; set; }

        public int playerScore2 { get; set; }

        public int playerScore3 { get; set; }
        public int playerScore4 { get; set; }
        public int playerScore5 { get; set; }
        public int playerScore6 { get; set; }
        public int playerScore7 { get; set; }
        public int playerScore8 { get; set; }
        public int playerScore9 { get; set; }

    }
}
