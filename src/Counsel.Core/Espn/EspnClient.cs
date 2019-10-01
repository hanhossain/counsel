using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Counsel.Core.Espn.Models;

namespace Counsel.Core.Espn
{
	public class EspnClient : IEspnClient
	{
		private readonly HttpClient _client;

		public EspnClient(HttpClient client)
		{
			_client = client;
		}

		public async Task<List<Season>> GetSeasonsAsync()
		{
			using var response = await _client.GetAsync("https://fantasy.espn.com/apis/v3/games/ffl/seasons");
			return await response.Content.ReadAsAsync<List<Season>>();
		}
	}
}
