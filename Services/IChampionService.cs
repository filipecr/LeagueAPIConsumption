using Newtonsoft.Json.Linq;

namespace LeagueAPIConsumption.Services
{
    public interface IChampionService
    {
        Task<Dictionary<string, JObject>> GetLatestChampionList();

        Task <string> GetChampionNameByKeyAsync(string key);
    }
}
