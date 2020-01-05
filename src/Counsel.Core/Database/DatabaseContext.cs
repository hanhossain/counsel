using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Counsel.Core.Database
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Player> Players { get; set; }

		public DbSet<Statistics> Statistics { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// needed for ios
			SQLitePCL.Batteries_V2.Init();

			// found path from:
			// https://stackoverflow.com/questions/47237414/what-is-the-best-environment-specialfolder-for-store-application-data-in-xamarin
			string databasePath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				"..",
				"Library",
				"database.db");

			optionsBuilder.UseSqlite($"Filename={databasePath}");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Statistics>()
				.HasKey(x => new { x.Season, x.Week, x.PlayerId });
		}
	}
}
