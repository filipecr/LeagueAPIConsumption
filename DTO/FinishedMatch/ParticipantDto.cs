namespace LeagueAPIConsumption.DTO
{
    public class ParticipantDto
    {
        public string puuid { get; set; }


        public string riotIdGameName { get; set; }
        public string riotIdTagline { get; set; }
        public string championName { get; set; }

        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }

        public string summonerLevel { get; set; }
        public string teamPosition { get; set; }
        public int stealthWardsPlaced { get; set; }
        public bool firstBloodKill { get; set; }

        public bool win {  get; set; }
       
    }
}
