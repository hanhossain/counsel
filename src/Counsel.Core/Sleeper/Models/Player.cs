using System;
using System.Collections.Generic;
using Counsel.Core.Sleeper.Converters;
using Newtonsoft.Json;

namespace Counsel.Core.Sleeper.Models
{
	public class Player
	{
		public string Hashtag { get; set; }

		[JsonIgnore]
		public string FullName => $"{FirstName} {LastName}";

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		public string Position { get; set; }

		[JsonProperty("search_full_name")]
		public string SearchFullName { get; set; }

		[JsonProperty("search_first_name")]
		public string SearchFirstName { get; set; }

		[JsonProperty("search_last_name")]
		public string SearchLastName { get; set; }

		public string Sport { get; set; }

		[JsonProperty("fantasy_positions")]
		public List<string> FantasyPositions { get; set; }

		[JsonProperty("player_id")]
		public string PlayerId { get; set; }

		public string College { get; set; }

		[JsonProperty("sportradar_id")]
		public string SportradarId { get; set; }

		public string Height { get; set; }

		public string Status { get; set; }

		[JsonProperty("gsis_id")]
		public string GsisId { get; set; }

		[JsonProperty("team")]
		public string Team { get; set; }

		[JsonProperty("injury_status")]
		public string InjuryStatus { get; set; }

		[JsonProperty("injury_body_part")]
		public string InjuryBodyPart { get; set; }

		[JsonProperty("injury_notes")]
		public string InjuryNotes { get; set; }

		[JsonProperty("years_exp")]
		public int? YearsExp { get; set; }

		public int? Age { get; set; }

		public int? Weight { get; set; }

		[JsonProperty("search_rank")]
		public int? SearchRank { get; set; }

		public int? Number { get; set; }

		[JsonProperty("rotowire_id")]
		public int? RotowireId { get; set; }

		[JsonProperty("birth_date")]
		public DateTime? BirthDate { get; set; }

		[JsonProperty("fantasy_data_id")]
		public int? FantasyDataId { get; set; }

		public bool Active { get; set; }

		[JsonProperty("yahoo_id")]
		public int? YahooId { get; set; }

		[JsonProperty("espn_id")]
		public int? EspnId { get; set; }

		[JsonConverter(typeof(UnixMillisecondDateTimeConverter))]
		[JsonProperty("news_updated")]
		public DateTime? NewsUpdated { get; set; }

		[JsonProperty("rotoworld_id")]
		public int? RotoworldId { get; set; }

		[JsonProperty("depth_chart_order")]
		public int? DepthChartOrder { get; set; }

		[JsonProperty("depth_chart_position")]
		public string DepthChartPosition { get; set; }

		[JsonProperty("stats_id")]
		public int? StatsId { get; set; }

		[JsonProperty("injury_start_date")]
		public DateTime? InjuryStartDate { get; set; }

		[JsonProperty("practice_description")]
		public string PracticeDescription { get; set; }

		[JsonProperty("practice_participation")]
		public string PracticeParticipation { get; set; }
	}
}
