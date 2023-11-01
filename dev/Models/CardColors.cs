/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	public class CardColors
	{
		/// <summary>Card's identifier.</summary>
		public int CardId { get; set; }

		/// <summary>Card object.</summary>
		public Card Card { get; set; }

		/// <summary>Color's identifier.</summary>
		public int ColorId { get; set; }

		/// <summary>Color object.</summary>
		public Color Color { get; set; }
	}
}
