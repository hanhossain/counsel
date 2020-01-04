using System;
using System.Collections.Generic;

namespace Counsel.Core.Nfl
{
	public class NflStats
	{
		public string Season { get; set; }

		public string Week { get; set; }

		public List<NflPlayerStats> Players { get; set; }
	}
}
