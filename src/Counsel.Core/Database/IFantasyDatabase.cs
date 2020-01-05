using System.Collections.Generic;
using System.Threading.Tasks;
using Counsel.Core.Nfl;

namespace Counsel.Core.Database
{
	public interface IFantasyDatabase
	{
		// TODO: update this with season + week
		Task<bool> PlayersExistAsync();

		Task UpdatePlayersAsync(int season, int week, IEnumerable<NflAdvancedStats> advancedStats, IDictionary<string, NflPlayerStats> weekStats);

		Task<IEnumerable<Player>> GetPlayersAsync();
	}
}
