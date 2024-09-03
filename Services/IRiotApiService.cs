using LeagueAPIConsumption.DTO;
using LeagueAPIConsumption.DTO.FinishedMatch;
using LeagueAPIConsumption.DTO.LiveMatch;

namespace LeagueAPIConsumption.Services
{
    public interface IRiotApiService
    {
        Task<Summoner> GetSummonerIdByNameAsync(string summonerName, string tagline);

        Task<CurrentGameInfo> GetCurrentGameInfoAsync(string puuid, string region);

        Task<List<string>> GetMatchesbySummonerID(string puuid);

        Task<List<MatchDto>> FetchMatchListBySummonerAsync(string puuid);

        Task<MatchDto> FetchMatchDetailsAsync(string matchId);
    }
}
