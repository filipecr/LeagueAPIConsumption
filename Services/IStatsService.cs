using LeagueAPIConsumption.DTO.FinishedMatch;

namespace LeagueAPIConsumption.Services
{
    public interface IStatsService
    {
        double CalculateWinRate(List<MatchDto> matches, string puuid);

        Task<double> GetWinRateAsync(string puuid);
    }
}
