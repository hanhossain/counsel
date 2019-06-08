using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Counsel.FantasyClient.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent)
        {
            string content = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
