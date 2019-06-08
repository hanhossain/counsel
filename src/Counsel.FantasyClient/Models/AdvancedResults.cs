using System.Collections.Generic;
using Newtonsoft.Json;

namespace Counsel.FantasyClient.Models
{
    public class AdvancedResults
    {
        [JsonProperty("QB")]
        public List<AdvancedPlayerResult> Quarterbacks { get; set; }

        [JsonProperty("RB")]
        public List<AdvancedPlayerResult> RunningBacks { get; set; }

        [JsonProperty("WR")]
        public List<AdvancedPlayerResult> WideReceivers { get; set; }

        [JsonProperty("TE")]
        public List<AdvancedPlayerResult> TightEnds { get; set; }

        [JsonProperty("K")]
        public List<AdvancedPlayerResult> Kickers { get; set; }

        [JsonProperty("DEF")]
        public List<AdvancedPlayerResult> Defenses { get; set; }
    }
}
