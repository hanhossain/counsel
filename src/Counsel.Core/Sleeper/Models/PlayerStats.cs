using System;
using Newtonsoft.Json;

namespace Counsel.Core.Sleeper.Models
{
	public class PlayerStats
	{
		[JsonProperty("pts_std")]
		public double? Points { get; set; }
	}
}
