using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Counsel.FantasyClient.Exceptions;
using Counsel.FantasyClient.Extensions;
using Counsel.FantasyClient.Models;
using Newtonsoft.Json;

namespace Counsel.FantasyClient
{
    public class NflFantasyClient
    {
        private readonly HttpClient _client;

        public NflFantasyClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri("https://api.fantasy.nfl.com/v1/players/");
        }

        public Task<AdvancedResults> GetAdvancedResultsAsync(int season, int week)
        {
            return SendAsync<AdvancedResults>($"advanced?format=json&season={season}&week={week}");
        }

        private async Task<T> SendAsync<T>(string path)
        {
            using (var response = await _client.GetAsync(path))
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return await response.Content.ReadAsAsync<T>();

                    case HttpStatusCode.BadRequest:
                        var json = await response.Content.ReadAsStringAsync();
                        var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(json);
                        throw new ClientException(response.StatusCode, errorResponse["errors"]);

                    default:
                        throw new ClientException(response.StatusCode);
                }
            }
        }
    }
}
