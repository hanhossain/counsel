using System.Collections.Generic;
using System.Linq;

namespace Counsel.Core.Models
{
	public class PlayerStats
	{
		public List<int> Weeks { get; set; }

		public List<(double Points, double Projected)> Points { get; set; }

		public double Average { get; set; }

		public double Max { get; set; }

		public double Min { get; set; }

		public double Range { get; set; }

		public double PopStdDev { get; set; }
	}
}
