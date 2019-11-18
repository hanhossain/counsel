using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Counsel.Core.Espn;
using Counsel.Core.Models;
using Counsel.Core.Nfl;
using Counsel.Core.Sleeper;
using SleeperModels = Counsel.Core.Sleeper.Models;

namespace Counsel.Core
{
	public class FantasyService : IFantasyService
	{
		private static readonly HashSet<string> _offensivePositions = new HashSet<string>() { "QB", "RB", "WR", "TE", "K" };
		private static readonly HashSet<string> _defensivePositions = new HashSet<string>() { "DEF" };
		private readonly AsyncLock _asyncLock = new AsyncLock();
		private readonly ISleeperClient _sleeperClient;
		private readonly IEspnClient _espnClient;
		private readonly INflClient _nflClient;

		private Dictionary<(int Season, int Week), List<NflAdvancedStats>> _advancedStats = new Dictionary<(int, int), List<NflAdvancedStats>>();

		private List<Dictionary<string, SleeperModels.PlayerStats>> _stats;
		private List<Dictionary<string, SleeperModels.PlayerStats>> _projectedStats;
		private Dictionary<string, SleeperModels.Player> _players;
		private int? _season;
		private int? _week;

		public FantasyService(ISleeperClient sleeperClient, IEspnClient espnClient, INflClient nflClient)
		{
			_sleeperClient = sleeperClient;
			_espnClient = espnClient;
			_nflClient = nflClient;
		}

		public async Task<Dictionary<string, Player>> GetPlayersAsync()
		{
			if (_players == null)
			{
				using var lockToken = await _asyncLock.LockAsync();

				if (_players == null)
				{
					var validPositions = _offensivePositions.Union(_defensivePositions).ToHashSet();

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
				}).ToList(),
				Season = season
			};

			// calculated stats requires at least one week to have passed
			if (week > 1)
			{
				var points = result.Points.Select(x => x.Points).ToList();

				result.Average = points.Average();
				result.Max = points.Max();
				result.Min = points.Min();
				result.Range = result.Max - result.Min;
				result.PopStdDev = Math.Sqrt(points.Sum(x => Math.Pow(x - result.Average, 2)) / points.Count);
			}
			
			return result;
		}

		public async Task<bool> ContainsStatsAsync()
		{
			using var lockToken = await _asyncLock.LockAsync();
			return _stats != null;
		}

		public async Task<Dictionary<string, Player>> SearchPlayersAsync(string playerName)
		{
			using var lockToken = await _asyncLock.LockAsync();
			return _players.Values
				.Where(x => x.FullName.Contains(playerName, StringComparison.OrdinalIgnoreCase))
				.Select(x => new Player()
				{
					FirstName = x.FirstName,
					LastName = x.LastName,
					Id = x.PlayerId,
					Position = x.Position,
					Team = x.Team
				})
				.ToDictionary(x => x.Id, x => x);
		}

		public async Task<List<Player>> GetOpponentsAsync(string playerId, int season, int week)
		{
			using var lockToken = await _asyncLock.LockAsync();

			var key = (season, week);

			if (!_advancedStats.TryGetValue(key, out var advancedWeekStats))
			{
				var advancedStatsResponse = await _nflClient.GetAdvancedStatsAsync(season, week);
				advancedWeekStats = advancedStatsResponse.Values.SelectMany(x => x).ToList();
				_advancedStats[key] = advancedWeekStats;
			}

			// get player stats
			var player = _players[playerId];
			var isDefensivePlayer = _defensivePositions.Contains(player.Position);

			var playerAdvancedStats = isDefensivePlayer
				? advancedWeekStats.Single(x => x.FirstName == player.FirstName && x.LastName == player.LastName && x.Team == player.Team)
				: advancedWeekStats.Single(x => x.GsisPlayerId == player.GsisId);

			// get opp team
			var opposingTeam = playerAdvancedStats.OpponentTeam.TrimStart('@');

			// get all players on opposing team
			var allOnOpposingTeam = _players.Values.Where(x => x.Team == opposingTeam);

			var opposingPosition = isDefensivePlayer ? _offensivePositions : _defensivePositions;
			var opposingPlayers = allOnOpposingTeam.Where(x => opposingPosition.Contains(x.Position));

			return opposingPlayers
				.OrderBy(x => x.LastName)
				.ThenBy(x => x.FirstName)
				.Select(x => new Player()
				{
					FirstName = x.FirstName,
					LastName = x.LastName,
					Id = x.PlayerId,
					Position = x.Position,
					Team = x.Team
				})
				.ToList();
		}
	}
}
