using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Net.Http
{
	public static class HttpContentExtensions
	{
		public static async Task<T> ReadAsAsync<T>(this HttpContent content)
		{
			string json = await content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
