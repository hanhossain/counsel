using System.Collections.Generic;
using System.Threading.Tasks;

namespace Counsel.Core.Nfl
{
	public interface INflClient
	{
		Task<Dictionary<string, List<NflAdvancedStats>>> GetAdvancedStatsAsync(int season, int week);

		Task<NflStats> GetStatsAsync(int season, int week);
	}
}
