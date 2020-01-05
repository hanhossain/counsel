using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Counsel.Core.Nfl;
using Microsoft.EntityFrameworkCore;

namespace Counsel.Core.Database
{
	public class FantasyDatabase : IFantasyDatabase
	{
		private readonly DatabaseContext _context;

		public FantasyDatabase(DatabaseContext context)
		{
			_context = context;
			_context.Database.Migrate();
		}

		public async Task<bool> PlayersExistAsync()
		{
			return await _context.Players.AnyAsync();
		}

		public async Task UpdatePlayersAsync(int season, int week, IEnumerable<NflAdvancedStats> advancedStats, IDictionary<string, NflPlayerStats> weekStats)
		{
			foreach (var playerAdvancedStats in advancedStats)
			{
				if (!await _context.Players.AnyAsync(x => x.Id == playerAdvancedStats.Id))
				{
					var dbPlayer = new Player()
					{
						Id = playerAdvancedStats.Id,
						FirstName = playerAdvancedStats.FirstName,
						LastName = playerAdvancedStats.LastName,
						Team = playerAdvancedStats.Team,
						Position = playerAdvancedStats.Position
					};

					_context.Players.Add(dbPlayer);
				}

				if (weekStats.TryGetValue(playerAdvancedStats.Id, out var playerWeekStats))
				{
					var dbStatistics = new Statistics()
					{
						PlayerId = playerAdvancedStats.Id,
						OpponentTeam = playerAdvancedStats.OpponentTeam,
						Season = season,
						Week = week,
						Points = playerWeekStats.WeekPoints,
						ProjectedPoints = playerWeekStats.WeekProjectedPoints
					};

					_context.Statistics.Add(dbStatistics);
				}
			}

			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Player>> GetPlayersAsync()
		{
			return await _context.Players.ToListAsync();
		}

		public async Task<IEnumerable<Statistics>> GetStatisticsAsync(string playerId)
		{
			var weeks = _context.Statistics.Select(x => x.Week).Distinct().ToList();
			return await _context.Statistics
				.Where(x => x.PlayerId == playerId)
				.OrderBy(x => x.Season)
				.ThenBy(x => x.Week)
				.ToListAsync();
		}

		public async Task<IEnumerable<Player>> SearchPlayersAsync(string query)
		{
			var players = await _context.Players.ToListAsync();

			return players
				.Where(x => $"{x.FirstName} {x.LastName}".Contains(query, StringComparison.OrdinalIgnoreCase))
				.ToList();
		}
	}
}
