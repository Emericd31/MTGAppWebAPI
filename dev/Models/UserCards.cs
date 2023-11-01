/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Class that manages object related to the intermediate table between Users and Cards.</summary>
	public class UserCards
	{
		/// <summary>User's identifier.</summary>
		public int UserId { get; set; }

		/// <summary>User object.</summary>
		public User User { get; set; }

		/// <summary>Card's identifier.</summary>
		public int CardId { get; set; }

		/// <summary>Card object.</summary>
		public Card Card { get; set; }

		/// <summary>Number of card in French language.</summary>
		public int FrenchNumber { get; set; }

		/// <summary>Number of foild card in French language.</summary>
		public int FrenchFoilNumber { get; set; }

		/// <summary>Number of card in English language.</summary>
		public int EnglishNumber { get; set; }

		/// <summary>Number of foild card in English language.</summary>
		public int EnglishFoilNumber { get; set; }
	}
}
