using System.Collections.Generic;
using System.Threading.Tasks;
using Counsel.Core.Models;

namespace Counsel.Core
{
	public interface IFantasyService
	{
		Task UpdateAsync();

		Task<Dictionary<string, Player>> GetPlayersAsync();

		Task<(int Season, int Week)> GetCurrentWeekAsync();

		Task<PlayerStats> GetStatsAsync(string playerId);

		Task<Dictionary<string, Player>> SearchPlayersAsync(string playerName);

		Task<Dictionary<string, List<(Player Player, double Points)>>> GetOpponentsAsync(string playerId, int season, int week);
	}
}
