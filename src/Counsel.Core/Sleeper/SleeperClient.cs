using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Counsel.Core.Sleeper.Models;

namespace Counsel.Core.Sleeper
{
	public class SleeperClient : ISleeperClient
	{
		private const string _baseAddress = "https://api.sleeper.app/v1";
		
		private readonly HttpClient _client;

		public SleeperClient(HttpClient client)
		{
			_client = client;
		}

		public async Task<Dictionary<string, Player>> GetPlayersAsync()
		{
			using var response = await _client.GetAsync($"{_baseAddress}/players/nfl");
			return await response.Content.ReadAsAsync<Dictionary<string, Player>>();
		}

		public async Task<Dictionary<string, Dictionary<string, double>>> GetSeasonStatsAsync(int season)
		{
			using var response = await _client.GetAsync($"{_baseAddress}/stats/nfl/regular/{season}");
			return await response.Content.ReadAsAsync<Dictionary<string, Dictionary<string, double>>>();
		}

		public async Task<Dictionary<string, Dictionary<string, double>>> GetWeekStatsAsync(int season, int week)
		{
			using var response = await _client.GetAsync($"{_baseAddress}/stats/nfl/regular/{season}/{week}");
			return await response.Content.ReadAsAsync<Dictionary<string, Dictionary<string, double>>>();
		}
	}
}
