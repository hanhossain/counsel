using System;
using System.Collections.Generic;

namespace Counsel.Core.Nfl
{
	public class NflStats
	{
		public int Season { get; set; }

		public int Week { get; set; }

		public List<NflPlayerStats> Players { get; set; }
	}
}
