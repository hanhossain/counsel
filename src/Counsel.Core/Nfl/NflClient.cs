using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Counsel.Core.Nfl
{
	public class NflClient : INflClient
	{
		private readonly HttpClient _client;

		public NflClient(HttpClient client)
		{
			_client = client;
		}

		public async Task<Dictionary<string, List<NflAdvancedStats>>> GetAdvancedStatsAsync(int season, int week)
		{
			using var response = await _client.GetAsync($"https://api.fantasy.nfl.com/v1/players/advanced?season={season}&week={week}&format=json&count=1000");
			return await response.Content.ReadAsAsync<Dictionary<string, List<NflAdvancedStats>>>();
		}
	}
}
