using System.Collections.Generic;
using System.Threading.Tasks;
using Counsel.Core.Sleeper.Models;

namespace Counsel.Core.Sleeper
{
	public interface ISleeperClient
	{
		Task<Dictionary<string, Player>> GetPlayersAsync();

		Task<Dictionary<string, Dictionary<string, double>>> GetSeasonStatsAsync(int season);

		Task<Dictionary<string, PlayerStats>> GetWeekStatsAsync(int season, int week);
	}
}
