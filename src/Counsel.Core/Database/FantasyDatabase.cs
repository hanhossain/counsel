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

		public async Task UpdatePlayersAsync(IEnumerable<NflAdvancedStats> players)
		{
			foreach (var player in players)
			{
				if (!await _context.Players.AnyAsync(x => x.Id == player.Id))
				{
					var dbPlayer = new Player()
					{
						Id = player.Id,
						FirstName = player.FirstName,
						LastName = player.LastName,
						Team = player.Team,
						Position = player.Position
					};

					_context.Players.Add(dbPlayer);
				}
			}

			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Player>> GetPlayersAsync()
		{
			return await _context.Players.ToListAsync();
		}
	}
}
