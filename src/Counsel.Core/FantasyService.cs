using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Counsel.Core.Espn;
using Counsel.Core.Models;
using Counsel.Core.Sleeper;
using SleeperModels = Counsel.Core.Sleeper.Models;

namespace Counsel.Core
{
	public class FantasyService : IFantasyService
	{
		private readonly AsyncLock _asyncLock = new AsyncLock();
		private readonly ISleeperClient _sleeperClient;
		private readonly IEspnClient _espnClient;

		private List<Dictionary<string, SleeperModels.PlayerStats>> _stats;
		private List<Dictionary<string, SleeperModels.PlayerStats>> _projectedStats;
		private Dictionary<string, SleeperModels.Player> _players;
		private int? _season;
		private int? _week;

		public FantasyService(ISleeperClient sleeperClient, IEspnClient espnClient)
		{
			_sleeperClient = sleeperClient;
			_espnClient = espnClient;
		}

		public async Task<Dictionary<string, Player>> GetPlayersAsync()
		{
			if (_players == null)
			{
				using var lockToken = await _asyncLock.LockAsync();

				if (_players == null)
				{
					var validPositions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
						{
							"QB",
							"RB",
							"WR",
							"TE",
							"K",
							"DEF"
						};

					var players = await _sleeperClient.GetPlayersAsync();

					_players = players.Values
						.Where(x => x.Active && validPositions.Contains(x.Position))
						.ToDictionary(x => x.PlayerId, x => x);
				}
			}

			return _players.Values.Select(x => new Player()
			{
				Id = x.PlayerId,
				FirstName = x.FirstName,
				LastName = x.LastName,
				Position = x.Position,
				Team = x.Team
			}).ToDictionary(x => x.Id, x => x);
		}

		public async Task<(int Season, int Week)> GetCurrentWeekAsync()
		{
			if (_season == null || _week == null)
			{
				using var lockToken = await _asyncLock.LockAsync();

				if(_season == null || _week == null)
				{
					var seasons = await _espnClient.GetSeasonsAsync();
					var recentSeason = seasons.First();
					_season = recentSeason.Id;
					_week = recentSeason.CurrentScoringPeriod.Id;
				}
			}

			return (_season.Value, _week.Value);
		}

		public async Task<PlayerStats> GetStatsAsync(string playerId)
		{
			(int season, int week) = await GetCurrentWeekAsync();

			if (_stats == null)
			{
				using var lockToken = await _asyncLock.LockAsync();

				if (_stats == null)
				{
					var tasks = Enumerable.Range(0, week)
						.Select(x => _sleeperClient.GetWeekStatsAsync(season, x + 1))
						.ToList();

					_stats = (await Task.WhenAll(tasks)).ToList();
				}
			}

			if (_projectedStats == null)
			{
				using var lockToken = await _asyncLock.LockAsync();

				if (_projectedStats == null)
				{
					var tasks = Enumerable.Range(0, week)
						.Select(x => _sleeperClient.GetProjectedWeekStatsAsync(season, x + 1))
						.ToList();

					_projectedStats = (await Task.WhenAll(tasks)).ToList();
				}
			}

			var result = new PlayerStats
			{
				Weeks = Enumerable.Range(1, week).ToList(),
				Points = _stats.Zip(_projectedStats, (stats, projected) =>
				{
					stats.TryGetValue(playerId, out var playerStats);
					projected.TryGetValue(playerId, out var playerProjected);
					return (playerStats?.Points ?? 0, playerProjected?.Points ?? 0);
				}).ToList()
			};

			return result;
		}

		public async Task<bool> ContainsStatsAsync()
		{
			using var lockToken = await _asyncLock.LockAsync();
			return _stats != null;
		}
	}
}
