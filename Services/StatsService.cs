using LeagueAPIConsumption.DTO.FinishedMatch;

namespace LeagueAPIConsumption.Services
{
    public class StatsService : IStatsService
    {
        private readonly IRiotApiService _riotservice;
       
        public StatsService(IRiotApiService riotservice)
        {
            _riotservice = riotservice;
        }

        public double CalculateWinRate(List<MatchDto> matches, string puuid)
        {
            int totalMatches = 0;
            int totalWins = 0;

            foreach (var matchDto in matches)
            {
                var participant = matchDto.info.participants.FirstOrDefault(p => p.puuid == puuid);

                if (participant != null)
                {

                    totalMatches++;
                    if (participant.win)
                    {
                        totalWins++;
                    }
                }
            }

            return (totalMatches > 0) ? (double)totalWins / totalMatches * 100 : 0;
        }

        public async Task<double> GetWinRateAsync(string puuid)
        {
            var matches =  await _riotservice.FetchMatchListBySummonerAsync(puuid);
            double winRate = CalculateWinRate(matches, puuid);
            return winRate;
        }



    }
}
