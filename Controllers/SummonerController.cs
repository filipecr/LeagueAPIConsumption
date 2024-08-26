using LeagueAPIConsumption.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueAPIConsumption.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SummonerController : ControllerBase
    {
        private readonly IRiotApiService _battleService;

        public SummonerController(IRiotApiService battleService)
        {
            _battleService = battleService;
        }

        [HttpGet("summonerID")]
        public async Task<IActionResult> Get(string summonerName,string tagline)
        {
            var result = await _battleService.GetSummonerIdByNameAsync(summonerName,tagline);
            return Ok(result);
        }

        [HttpGet("GetMatch")]

        public async Task<IActionResult> GetMatchInfo(string summonerName, string tagline, string region)
        {
            var resultId = await _battleService.GetSummonerIdByNameAsync(summonerName, tagline);
            string puuid = resultId.puuid;
            var resultCurrentGame = await _battleService.GetCurrentGameInfoAsync(puuid,region);
            return Ok(resultCurrentGame);
        }
    }

}
