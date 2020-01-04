using Newtonsoft.Json;

namespace Counsel.Core.Nfl
{
	public class NflPlayerStats
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Position { get; set; }

		[JsonProperty("teamAbbr")]
		public string Team { get; set; }

		[JsonProperty("weekPts")]
		public double WeekPoints { get; set; }

		[JsonProperty("weekProjectedPts")]
		public double WeekProjectedPoints { get; set; }
	}
}
