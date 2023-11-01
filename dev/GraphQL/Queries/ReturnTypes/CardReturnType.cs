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
	public class CardReturnType
	{
		public long NumberOfCards { get; set; }

		public List<Card> Cards { get; set; }

		public CardReturnType(long numberOfCards, List<Card> cards)
		{
			NumberOfCards = numberOfCards;
			Cards = cards;
		}
	}
}
