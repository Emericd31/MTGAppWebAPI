/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL.Queries.ReturnTypes
{
	/// <summary>Result of card queries.</summary>
	public class CardReturnType
	{
		#region Public Properties

		/// <summary>Number of resulting cards.</summary>
		public long NumberOfCards { get; set; }

		/// <summary>List of resulting cards.</summary>
		public List<Card> Cards { get; set; } = new List<Card>();

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public CardReturnType() { }

		/// <summary>Constructor.</summary>
		/// <param name="numberOfCards">Number of resulting cards.</param>
		/// <param name="cards">List of resulting cards.</param>
		public CardReturnType(long numberOfCards, List<Card> cards)
		{
			NumberOfCards = numberOfCards;
			Cards = cards;
		}

		#endregion Constructor
	}
}
