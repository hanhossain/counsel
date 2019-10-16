using System.Collections.Generic;
using UIKit;

namespace Counsel.iOS.Graphing
{
	public class LineData
	{
		public IEnumerable<ChartEntry> Entries { get; set; }

		public UIColor Color { get; set; }
	}
}
