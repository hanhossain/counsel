using System.Threading.Tasks;

namespace Counsel.iOS
{
	public interface ISearchDelegate
	{
		Task OnSearchAsync(string playerName);
	}
}
