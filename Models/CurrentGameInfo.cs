namespace LeagueAPIConsumption.Models
{
    public class CurrentGameInfo
    {
        public long GameId { get; set; }
        public string GameType { get; set; }
        public long GameStartTime { get; set; }
        public long MapId { get; set; }
        public long GameLength { get; set; }
        public string PlatformId { get; set; }
        public string GameMode { get; set; }
        public BannedChampion[] BannedChampions { get; set; }
        public Observer Observers { get; set; }
        public Participant[] Participants { get; set; }
        public long GameQueueConfigId { get; set; }
    }
}
