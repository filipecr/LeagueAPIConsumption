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

        [HttpGet("summonerID")]
        public async Task<IActionResult> Get(string summonerName,string tagline)
        {
            var result = await _riotApiService.GetSummonerIdByNameAsync(summonerName,tagline);
            return Ok(result);
        }

        [HttpGet("GetMatch")]

        public async Task<IActionResult> GetMatchInfo(string summonerName, string tagline, string region)
        {
            var resultId = await _riotApiService.GetSummonerIdByNameAsync(summonerName, tagline);
            string puuid = resultId.puuid;
            var resultCurrentGame = await _riotApiService.GetCurrentGameInfoAsync(puuid,region);
            return Ok(resultCurrentGame);
        }

        [HttpGet("TestId")]
        
        public async Task<IActionResult> GetChampionByKey(string key)
        {
            var result = await _championService.GetChampionNameByKeyAsync(key);
            return Ok(result);
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
