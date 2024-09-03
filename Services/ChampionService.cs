using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace LeagueAPIConsumption.Services
{
    public class ChampionService : IChampionService
    {
        private readonly HttpClient _httpClient;
        public ChampionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, JObject>> GetLatestChampionList()
        {

            try
            {
                    // Fetching the latest version from the API
                
                    var versionsResponse = await _httpClient.GetStringAsync("https://ddragon.leagueoflegends.com/api/versions.json");
                    var versions = JArray.Parse(versionsResponse);
                    var latestVersion = versions[0].ToString();

                    // Fetching the champions data for the latest version
                    var ddragonResponse = await _httpClient.GetStringAsync($"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/data/en_US/champion.json");

                    var champions = JObject.Parse(ddragonResponse)["data"].ToObject<Dictionary<string, JObject>>();

                    
                    return champions;
                             
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching DDragon data: {ex.Message}");
            }
            
        }

        public async Task<string> GetChampionNameByKeyAsync(string key)
        {
                var champions = await GetLatestChampionList();

                var champion = champions.FirstOrDefault(c => c.Value["key"]?.ToString() == key);

                
                if (!string.IsNullOrEmpty(champion.Key))
                {
                    return champion.Key;
                }

                throw new KeyNotFoundException($"Champion with key '{key}' not found.");
        }
    }
}
