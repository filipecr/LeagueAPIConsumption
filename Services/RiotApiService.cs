using LeagueAPIConsumption.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LeagueAPIConsumption.Services
{

    public class RiotApiService : IRiotApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RiotApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["RiotApi:ApiKey"];
        }


        public async Task<Summoner> GetSummonerIdByNameAsync(string summonerName, string tagline)
        {
            string requestUrl = ($"https://europe.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{summonerName}/{tagline}?api_key={_apiKey}");

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var summoner = JsonConvert.DeserializeObject<Summoner>(content);

            return summoner;
        }

        public async Task<CurrentGameInfo> GetCurrentGameInfoAsync(string puuid, string region)
        {
            switch (region)
            {
                case "euw":
                    region = "euw1";
                    break;

                case "na":
                    region = "na1";
                    break;
                default:
                    region = "euw1";
                    break;
            }

            string requestUrl = ($"https://{region}.api.riotgames.com/lol/spectator/v5/active-games/by-summoner/{puuid}?api_key={_apiKey}");
            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var currentGameInfo = JsonConvert.DeserializeObject<CurrentGameInfo>(content);

                return currentGameInfo;
            }
            catch (HttpRequestException ex)
            {

                throw new Exception($"Error fetching current game info: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {

                throw new Exception("Error deserializing the current game info response", ex);
            }
        }
    }


}
