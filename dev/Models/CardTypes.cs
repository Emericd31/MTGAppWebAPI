/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Card type object.</summary>
	public class CardTypes
	{
		#region Public Properties

		/// <summary>Card's identifier.</summary>
		public int CardId { get; set; }

		/// <summary>Card object.</summary>
		public Card? Card { get; set; }

		/// <summary>Type's identifier.</summary>
		public int TypeId { get; set; }

		/// <summary>Type object.</summary>
		public Type? Type { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public CardTypes() { }

		/// <summary>Constructor.</summary>
		/// <param name="card">Card.</param>
		/// <param name="type">Type.</param>
		public CardTypes(Card card, Type type)
		{
			Card = card;
			CardId = card.Id;
			Type = type;
			TypeId = type.Id;
		}

		#endregion Constructor
	}
}
