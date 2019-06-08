using Counsel.FantasyClient.Converters;
using Newtonsoft.Json;

namespace Counsel.FantasyClient.Models
{
    public class AdvancedPlayerStatistics
    {
        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? Carries { get; set; }

        [JsonProperty("fanPtsAgainstOpponentPts")]
        [JsonConverter(typeof(StringToNullableConverter<double>))]
        public double? FanPointsAgainstOpponentPoints { get; set; }

        [JsonProperty("fanPtsAgainstOpponentRank")]
        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? FanPointsAgainstOpponentRank { get; set; }

        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? ReceptionPercentage { get; set; }

        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? Receptions { get; set; }

        /// <summary>
        /// Number of red zone targets and touches when down is goal-to-go
        /// </summary>
        /// <value>The redzone goal to go.</value>
        [JsonProperty("redzoneG2g")]
        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? RedzoneGoalToGo { get; set; }

        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? RedzoneTargets { get; set; }

        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? RedzoneTouches { get; set; }

        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? Targets { get; set; }

        [JsonConverter(typeof(StringToNullableConverter<int>))]
        public int? Touches { get; set; }
    }
}
