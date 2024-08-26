using LeagueAPIConsumption.Models;

namespace LeagueAPIConsumption.Services
{
    public interface IRiotApiService
    {
        Task<Summoner> GetSummonerIdByNameAsync(string summonerName, string tagline);

        Task<CurrentGameInfo> GetCurrentGameInfoAsync(string puuid, string region);
    }
}
