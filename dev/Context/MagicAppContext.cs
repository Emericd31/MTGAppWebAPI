/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicAppAPI.Context
{
	public class MagicAppContext : DbContext
	{

		#region Constructor

		public MagicAppContext(DbContextOptions<MagicAppContext> options) : base(options)
		{
		}

		#endregion Constructor

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserRights>().HasKey(sc => new { sc.UserId, sc.RightId });
			modelBuilder.Entity<CardTypes>().HasKey(sc => new { sc.CardId, sc.TypeId });
			modelBuilder.Entity<CardColors>().HasKey(sc => new { sc.CardId, sc.ColorId });
			modelBuilder.Entity<CardKeywords>().HasKey(sc => new { sc.CardId, sc.KeywordId });
			modelBuilder.Entity<UserCards>().HasKey(sc => new { sc.UserId, sc.CardId });
		}

		#region DbSets

		/// <summary>Users.</summary>
		public DbSet<User> Users { get; set; }

		/// <summary>Rights.</summary>
		public DbSet<Right> Rights { get; set; }

		/// <summary>Intermediate table between Users and Rights.</summary>
		public DbSet<UserRights> UserRights { get; set; }

		/// <summary>Cards.</summary>
		public DbSet<Card> Cards { get; set; }

		/// <summary>Card types.</summary>
		public DbSet<Models.Type> Types { get; set; }

		/// <summary>Intermediate table between Cards and Types.</summary>
		public DbSet<CardTypes> CardTypes { get; set; }

		/// <summary>Card colors.</summary>
		public DbSet<Color> Colors { get; set; }

		/// <summary>Intermediate table between Cards and Colors.</summary>
		public DbSet<CardColors> CardColors { get; set; }

		/// <summary>Card keywords.</summary>
		public DbSet<Keyword> Keywords { get; set; }

		/// <summary>Intermediate table between Cards and Keywords.</summary>
		public DbSet<CardKeywords> CardKeywords { get; set; }

		/// <summary>Intermediate table between Users and Cards.</summary>
		public DbSet<UserCards> UserCards { get; set; }

		#endregion DbSets
	}
}
