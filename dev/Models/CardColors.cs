/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Card color object.</summary>
	public class CardColors
	{
		#region Public Properties

		/// <summary>Card's identifier.</summary>
		public int CardId { get; set; }

		/// <summary>Card object.</summary>
		public Card? Card { get; set; }

		/// <summary>Color's identifier.</summary>
		public int ColorId { get; set; }

		/// <summary>Color object.</summary>
		public Color? Color { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public CardColors() { }

		/// <summary>Constructor.</summary>
		/// <param name="card">Card.</param>
		/// <param name="color">Color.</param>
		public CardColors(Card card, Color color)
		{
			Card = card;
			CardId = card.Id;
			Color = color;
			ColorId = color.Id;
		}

		#endregion Constructor
	}
}
