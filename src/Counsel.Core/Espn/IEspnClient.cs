using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Counsel.Core.Espn.Models;

namespace Counsel.Core.Espn
{
	public interface IEspnClient
	{
		Task<List<Season>> GetSeasonsAsync();
	}
}
