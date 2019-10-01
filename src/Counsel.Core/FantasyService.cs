﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Counsel.Core.Espn;
using Counsel.Core.Models;
using Counsel.Core.Sleeper;
using SleeperPlayer = Counsel.Core.Sleeper.Models.Player;

namespace Counsel.Core
{
	public class FantasyService : IFantasyService
	{
		private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
		private readonly ISleeperClient _sleeperClient;
		private readonly IEspnClient _espnClient;

		private List<Dictionary<string, Dictionary<string, double>>> _stats;
		private Dictionary<string, SleeperPlayer> _players;
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
				await _lock.WaitAsync();
				
				try
				{
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
				finally
				{
					_lock.Release();
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
				await _lock.WaitAsync();

				try
				{
					if (_season == null || _week == null)
					{
						var seasons = await _espnClient.GetSeasonsAsync();
						var recentSeason = seasons.First();
						_season = recentSeason.Id;
						_week = recentSeason.CurrentScoringPeriod.Id;
					}
				}
				finally
				{
					_lock.Release();
				}
			}

			return (_season.Value, _week.Value);
		}

		public async Task<Dictionary<string, List<(int, double)>>> GetStatsAsync(string playerId)
		{
			(int season, int week) = await GetCurrentWeekAsync();

			if (_stats == null)
			{
				await _lock.WaitAsync();

				try
				{
					if (_stats == null)
					{
						var tasks = Enumerable.Range(0, week)
							.Select(x => _sleeperClient.GetWeekStatsAsync(season, x + 1))
							.ToList();

						_stats = (await Task.WhenAll(tasks)).ToList();
					}
				}
				finally
				{
					_lock.Release();
				}
			}

			var result = new Dictionary<string, List<(int, double)>>();

			for (int i = 0; i < _stats.Count; i++)
			{
				if (_stats[i].TryGetValue(playerId, out var weekStats))
				{
					foreach (var stat in weekStats)
					{
						if (!result.TryGetValue(stat.Key, out var values))
						{
							values = new List<(int, double)>();
						}

						values.Add((i + 1, stat.Value));
						result[stat.Key] = values;
					}
				}
			}

			return result;
		}
	}
}
