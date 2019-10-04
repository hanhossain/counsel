namespace Counsel.Core.Espn.Models
{
	public class Season
	{
		public bool Active { get; set; }

		public ScoringPeriod CurrentScoringPeriod { get; set; }

		public int Id { get; set; }

		public class ScoringPeriod
		{
			public int Id { get; set; }
		}
	}
}
