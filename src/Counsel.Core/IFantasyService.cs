using System.Collections.Generic;
using System.Threading.Tasks;
using Counsel.Core.Models;

namespace Counsel.Core
{
	public interface IFantasyService
	{
		Task<Dictionary<string, Player>> GetPlayersAsync();

		Task<(int Season, int Week)> GetCurrentWeekAsync();

		Task<Dictionary<string, List<(int, double)>>> GetStatsAsync(string playerId);
	}
}
