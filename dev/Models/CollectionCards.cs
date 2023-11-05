/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Collection card object.</summary>
	public class CollectionCards
	{
		#region Public Properties

		/// <summary>Collection's identifier.</summary>
		public int CollectionId { get; set; }

		/// <summary>Collection object.</summary>
		public Collection? Collection { get; set; }

		/// <summary>Card's identifier.</summary>
		public int CardId { get; set; }

		/// <summary>Card object.</summary>
		public Card? Card { get; set; }

		/// <summary>Number of card in French language.</summary>
		public int FrenchNumber { get; set; }

		/// <summary>Number of foil card in French language.</summary>
		public int FrenchFoilNumber { get; set; }

		/// <summary>Number of card in English language.</summary>
		public int EnglishNumber { get; set; }

		/// <summary>Number of foil card in English language.</summary>
		public int EnglishFoilNumber { get; set; }

		#endregion Public Properties
	}
}
