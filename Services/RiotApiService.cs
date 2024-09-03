using Azure;
using LeagueAPIConsumption.DTO;
using LeagueAPIConsumption.DTO.FinishedMatch;
using LeagueAPIConsumption.DTO.LiveMatch;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<List<string>> GetMatchesbySummonerID(string puuid)
        {
            // Initialize the list to avoid returning null in case of an exception
            List<string> matchIds = new List<string>();
            try
            {
                // Get the current time in UTC and calculate the Unix timestamps for 30 days ago
                DateTimeOffset currentTime = DateTimeOffset.UtcNow;
                DateTimeOffset startTime = currentTime.AddDays(-30);

                long unixStartTime = startTime.ToUnixTimeSeconds();
                long unixEndTime = currentTime.ToUnixTimeSeconds(); // Use current time as end time instead of startTime

                // Make the HTTP request to the Riot API
                string url = $"https://europe.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids" +
                             $"?queue=420&start=0&count=10&startTime={unixStartTime}&endTime={unixEndTime}&api_key={_apiKey}";

                var httpResponse = await _httpClient.GetStringAsync(url);

                // Deserialize the response into a List<string>
                if (!string.IsNullOrEmpty(httpResponse))
                {
                    matchIds = JsonConvert.DeserializeObject<List<string>>(httpResponse);
                }
                else
                {
                    // Log or handle the case where the response was empty
                    Console.WriteLine("No matches found for the given summoner ID.");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP request errors (e.g., network issues, API down)
                Console.WriteLine($"HTTP Request error: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                // Handle errors during JSON deserialization
                Console.WriteLine($"JSON Deserialization error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Return the list of match IDs, which could be empty if an error occurred
            return matchIds;
        }

        public async Task<List<MatchDto>> FetchMatchListBySummonerAsync(string puuid)
        {
            // Retrieve match history IDs based on the summoner's PUUID
            List<string> matchHistoryIds = await GetMatchesbySummonerID(puuid);

            // Initialize a list to hold the tasks that will fetch match details
            var tasks = new List<Task<MatchDto>>();

            try
            {
                // Iterate over each match ID and initiate a task to fetch match details
                foreach (var matchId in matchHistoryIds)
                {
                    // Add the task to the list of tasks
                    tasks.Add(FetchMatchDetailsAsync(matchId));
                }

                // Wait for all tasks to complete and gather the results
                var matches = await Task.WhenAll(tasks);

                // Filter out any null results (in case some matches couldn't be fetched) and return the list
                return matches.Where(match => match != null).ToList();
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request error: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Deserialization error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            return new List<MatchDto>();

        }

        public async Task<MatchDto> FetchMatchDetailsAsync(string matchId)
        {
            try
            {
                // Construct the URL
                string url = $"https://europe.api.riotgames.com/lol/match/v5/matches/{matchId}?api_key={_apiKey}";

                // Send the HTTP request
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Check if the response was successful
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                
                // Deserialize the response content into a MatchDto object
               var matchDto = JsonConvert.DeserializeObject<MatchDto>(content);
                return matchDto;


            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception($"HTTP Request error: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                throw new Exception($"JSON Deserialization error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
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
