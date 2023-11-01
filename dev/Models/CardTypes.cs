/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Class that manages object related to the intermediate table between Cards and Types.</summary>
	public class CardTypes
	{
		/// <summary>Card's identifier.</summary>
		public int CardId { get; set; }

		/// <summary>Card object.</summary>
		public Card Card { get; set; }

		/// <summary>Type's identifier.</summary>
		public int TypeId { get; set; }

		/// <summary>Type object.</summary>
		public Type Type { get; set; }
	}
}
