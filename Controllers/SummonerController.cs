using LeagueAPIConsumption.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueAPIConsumption.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SummonerController : ControllerBase
    {
        private readonly IRiotApiService _riotApiService;
        private readonly IChampionService _championService;
        private readonly IStatsService _statsService;
        private readonly ILogger<SummonerController> _logger;

        public SummonerController(IRiotApiService riotApiService, IStatsService statsService, ILogger<SummonerController> logger)
        {
            _riotApiService = riotApiService;
            _statsService = statsService;
            _logger = logger;
        }

        [HttpGet("GetWinrate")]
        public async Task<IActionResult> GetWinrate(string summonerName, string tagline)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(summonerName) || string.IsNullOrWhiteSpace(tagline))
            {
                return BadRequest("Summoner name and tagline are required.");
            }

            try
            {
                // Fetch the summoner ID
                var resultId = await _riotApiService.GetSummonerIdByNameAsync(summonerName, tagline);

                // Check if the summoner ID was successfully retrieved
                if (resultId == null || string.IsNullOrEmpty(resultId.puuid))
                {
                    return NotFound("Summoner not found.");
                }

                string puuid = resultId.puuid;

                var winrate = await _statsService.GetWinRateAsync(puuid);
                return Ok(winrate);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request error occurred while fetching winrate.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Error fetching data from the Riot API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching winrate.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

    }

}
