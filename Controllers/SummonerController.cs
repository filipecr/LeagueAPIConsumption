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

        public SummonerController(IRiotApiService riotApiService, IChampionService championService, IStatsService statsService)
        {
            _riotApiService = riotApiService;
            _championService = championService;
            _statsService = statsService;
        }       

        [HttpGet("GetWinrate")]

        public async Task<IActionResult> GetWinrate(string summonerName, string tagline)
        {
            var resultId = await _riotApiService.GetSummonerIdByNameAsync(summonerName, tagline);
            string puuid = resultId.puuid;
            var winrate =  await _statsService.GetWinRateAsync(puuid);

            return Ok(winrate);
        }
    }

}
