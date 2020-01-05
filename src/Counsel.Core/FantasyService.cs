using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Counsel.Core.Espn;
using Counsel.Core.Models;
using Counsel.Core.Nfl;

namespace Counsel.Core
{
	public class FantasyService : IFantasyService
	{
		private readonly AsyncLock _asyncLock = new AsyncLock();
		private readonly Database.IFantasyDatabase _fantasyDatabase;
		private readonly IEspnClient _espnClient;
		private readonly INflClient _nflClient;

		private int? _season;
		private int? _week;

		public FantasyService(Database.IFantasyDatabase fantasyDatabase, IEspnClient espnClient, INflClient nflClient)
		{
			_fantasyDatabase = fantasyDatabase;
			_espnClient = espnClient;
			_nflClient = nflClient;
		}

		public async Task UpdateAsync()
		{
			(int season, int week) = await GetCurrentWeekAsync();

			if (!await _fantasyDatabase.PlayersExistAsync())
			{
				var advancedSeasonTasks = Enumerable.Range(1, week)
					.Select(x => _nflClient.GetAdvancedStatsAsync(season, x))
					.ToList();

				var advancedSeasonStats = await Task.WhenAll(advancedSeasonTasks);

				var statsTasks = Enumerable.Range(1, week)
					.Select(x => _nflClient.GetStatsAsync(season, x))
					.ToList();

				var seasonStats = await Task.WhenAll(statsTasks);

				foreach (var (advancedWeekStats, weekStats) in advancedSeasonStats.Zip(seasonStats, (x, y) => (x, y)))
				{
					var a = weekStats.Players.ToDictionary(x => x.Id, x => x);
					await _fantasyDatabase.UpdatePlayersAsync(
						season,
						week,
						advancedWeekStats.Values.SelectMany(x => x),
						weekStats.Players.ToDictionary(x => x.Id, x => x));
				}
			}
		}

		public async Task<Dictionary<string, Player>> GetPlayersAsync()
		{
			var players = await _fantasyDatabase.GetPlayersAsync();

			return players.Select(x => new Player()
			{
				Id = x.Id,
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
			//(int season, int week) = await GetCurrentWeekAsync();

			//if (_stats == null)
			//{
			//	using var lockToken = await _asyncLock.LockAsync();

			//	if (_stats == null)
			//	{
			//		var tasks = Enumerable.Range(0, week)
			//			.Select(x => _sleeperClient.GetWeekStatsAsync(season, x + 1))
			//			.ToList();

			//		_stats = (await Task.WhenAll(tasks)).ToList();
			//	}
			//}

			//if (_projectedStats == null)
			//{
			//	using var lockToken = await _asyncLock.LockAsync();

			//	if (_projectedStats == null)
			//	{
			//		var tasks = Enumerable.Range(0, week)
			//			.Select(x => _sleeperClient.GetProjectedWeekStatsAsync(season, x + 1))
			//			.ToList();

			//		_projectedStats = (await Task.WhenAll(tasks)).ToList();
			//	}
			//}

			//var result = new PlayerStats
			//{
			//	Weeks = Enumerable.Range(1, week).ToList(),
			//	Points = _stats.Zip(_projectedStats, (stats, projected) =>
			//	{
			//		stats.TryGetValue(playerId, out var playerStats);
			//		projected.TryGetValue(playerId, out var playerProjected);
			//		return (playerStats?.Points ?? 0, playerProjected?.Points ?? 0);
			//	}).ToList(),
			//	Season = season,
			//	PlayerId = playerId
			//};

			//// calculated stats requires at least one week to have passed
			//if (week > 1)
			//{
			//	var points = result.Points.Select(x => x.Points).ToList();

			//	result.Mean = points.Average();
			//	result.Median = GetMedian(points);
			//	result.Max = points.Max();
			//	result.Min = points.Min();
			//	result.Range = result.Max - result.Min;
			//	result.PopStdDev = Math.Sqrt(points.Sum(x => Math.Pow(x - result.Mean, 2)) / points.Count);
			//}

			//return result;

			throw new NotImplementedException();
		}

		public async Task<bool> ContainsStatsAsync()
		{
			//using var lockToken = await _asyncLock.LockAsync();
			//return _stats != null;

			throw new NotImplementedException();
		}

		public async Task<Dictionary<string, Player>> SearchPlayersAsync(string playerName)
		{
			//using var lockToken = await _asyncLock.LockAsync();
			//return _players.Values
			//	.Where(x => x.FullName.Contains(playerName, StringComparison.OrdinalIgnoreCase))
			//	.Select(x => new Player()
			//	{
			//		FirstName = x.FirstName,
			//		LastName = x.LastName,
			//		Id = x.PlayerId,
			//		Position = x.Position,
			//		Team = x.Team
			//	})
			//	.ToDictionary(x => x.Id, x => x);

			throw new NotImplementedException();
		}

		public async Task<Dictionary<string, List<Player>>> GetOpponentsAsync(string playerId, int season, int week)
		{
			//using var lockToken = await _asyncLock.LockAsync();

			//var key = (season, week);

			//if (!_advancedStats.TryGetValue(key, out var advancedWeekStats))
			//{
			//	var advancedStatsResponse = await _nflClient.GetAdvancedStatsAsync(season, week);
			//	advancedWeekStats = advancedStatsResponse.Values.SelectMany(x => x).ToList();
			//	_advancedStats[key] = advancedWeekStats;
			//}

			//// get player stats
			//var player = _players[playerId];
			//var isDefensivePlayer = _defensivePositions.Contains(player.Position);

			//var playerAdvancedStats = isDefensivePlayer
			//	? advancedWeekStats.Single(x => x.FirstName == player.FirstName && x.LastName == player.LastName && x.Team == player.Team)
			//	: advancedWeekStats.Single(x => x.GsisPlayerId == player.GsisId);

			//// get opp team
			//var opposingTeam = playerAdvancedStats.OpponentTeam.TrimStart('@');

			//// get all players on opposing team
			//var allOnOpposingTeam = _players.Values.Where(x => x.Team == opposingTeam);

			//var opposingPosition = isDefensivePlayer ? _offensivePositions : _defensivePositions;
			//var opposingPlayers = allOnOpposingTeam.Where(x => opposingPosition.Contains(x.Position));

			//return opposingPlayers
			//	.Select(x => new Player()
			//	{
			//		FirstName = x.FirstName,
			//		LastName = x.LastName,
			//		Id = x.PlayerId,
			//		Position = x.Position,
			//		Team = x.Team
			//	})
			//	.GroupBy(x => x.Position)
			//	.ToDictionary(
			//		x => x.Key,
			//		x => x.OrderBy(y => y.LastName)
			//			.ThenBy(y => y.FirstName)
			//			.ToList());

			throw new NotImplementedException();
		}

		private static double GetMedian(List<double> values)
		{
			if (!values.Any())
			{
				throw new InvalidOperationException($"{nameof(values)} contains no elements");
			}

			var sorted = values.OrderBy(x => x).ToList();
			int middleIndex = sorted.Count / 2;
			double middleValue = sorted[middleIndex];

			if (sorted.Count % 2 == 0)
			{
				double otherMiddle = sorted[middleIndex + 1];

				middleValue = (middleValue + otherMiddle) / 2.0;
			}

			return middleValue;
		}
	}
}
