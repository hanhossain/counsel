using Counsel.FantasyClient.Converters;
using Newtonsoft.Json;

namespace Counsel.FantasyClient.Models
{
    public class AdvancedPlayerResult
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonProperty("teamAbbr")]
        public string Team { get; set; }

        [JsonProperty("opponentTeamAbbr")]
        public string OpponentTeam { get; set; }

        [JsonConverter(typeof(PositionConverter))]
        public Position Position { get; set; }

        [JsonProperty("stats")]
        public AdvancedPlayerStatistics Statistics { get; set; }

        public string Status { get; set; }
    }
}
