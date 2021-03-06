﻿using Newtonsoft.Json;

namespace Counsel.Core.Nfl
{
	public class NflAdvancedStats
	{
		public string Id { get; set; }

		[JsonProperty("opponentTeamAbbr")]
		public string OpponentTeam { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		[JsonProperty("teamAbbr")]
		public string Team { get; set; }

		public string Position { get; set; }
	}
}
