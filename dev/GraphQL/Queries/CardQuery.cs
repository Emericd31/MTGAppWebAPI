/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using HotChocolate.Authorization;
using MagicAppAPI.Bll;
using MagicAppAPI.Context;
using MagicAppAPI.ExternalAPIs.MockData;
using MagicAppAPI.ExternalAPIs.ScryFall;
using MagicAppAPI.GraphQL.Queries.ReturnTypes;
using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL.Queries
{
	[ExtendObjectType("Query")]
	public class CardQuery
	{
		[Authorize]
		/// <summary>Gets all cards in specific set.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="setCode">Set code.</param>
		/// <param name="includeExtras">Boolean indicating if the request should include extra cards.</param>
		/// <param name="includeVariations">Boolean indicating if the request should include variations cards.</param>
		/// <returns>List of cards.</returns>
		public async Task<CardReturnType> GetCardsBySetCode([Service] MagicAppContext context, string setCode, bool includeExtras = true, bool includeVariations = true)
		{
			var result = new CardReturnType(0, new List<Card>());

			using (BllCard bll = new BllCard(context))
			{
				var res = bll.GetCardsBySetCode(ScryFallRestClient.GetInstance(), setCode, includeExtras, includeVariations);
				result.NumberOfCards = res.numberOfCards;
				result.Cards = res.cards;
			}

			return result;
		}

		[Authorize]
		/// <summary>Gets all cards given code in specific set.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="code">Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public async Task<CardReturnType> GetCardsByCode([Service] MagicAppContext context, string code, string setCode)
		{
			var result = new CardReturnType(0, new List<Card>());

			using (BllCard bll = new BllCard(context))
			{
				var res = bll.GetCardsByCode(ScryFallRestClient.GetInstance(), code, setCode);
				result.NumberOfCards = res.numberOfCards;
				result.Cards = res.cards;
			}

			return result;
		}

		[Authorize]
		/// <summary>Gets all cards given code in specific set.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="mtgCode">MTG Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public async Task<CardReturnType> GetCardsByMTGCode([Service] MagicAppContext context, string mtgCode, string setCode)
		{
			var result = new CardReturnType(0, new List<Card>());

			using (BllCard bll = new BllCard(context))
			{
				var res = bll.GetCardsByMTGCode(ScryFallRestClient.GetInstance(), mtgCode, setCode);
				result.NumberOfCards = res.numberOfCards;
				result.Cards = res.cards;
			}

			return result;
		}

		[Authorize]
		/// <summary>Gets all cards given card unique identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="cardUID">Card unique identifier.</param>
		/// <returns>List of cards.</returns>
		public CardReturnType GetCardsByUID([Service] MagicAppContext context, string cardUID)
		{
			var result = new CardReturnType(0, new List<Card>());

			var card = context.Cards.FirstOrDefault(card => card.UID == cardUID);
			if (card != null)
			{
				// Get card colors
				var cardColors = context.CardColors.Where(c => c.CardId == card.Id).ToList();
				foreach (var cardColor in cardColors)
				{
					var color = context.Colors.FirstOrDefault(c => c.CardColors.Contains(cardColor));
					if (color != null)
						card.Colors.Add(color);
				}

				// Get card types 
				var cardTypes = context.CardTypes.Where(c => c.CardId == card.Id).ToList();
				foreach (var cardType in cardTypes)
				{
					var type = context.Types.FirstOrDefault(c => c.CardTypes.Contains(cardType));
					if (type != null)
						card.Types.Add(type);
				}

				// Get card keywords 
				var cardKeywords = context.CardKeywords.Where(c => c.CardId == card.Id).ToList();
				foreach (var cardKeyword in cardKeywords)
				{
					var keyword = context.Keywords.FirstOrDefault(c => c.CardKeywords.Contains(cardKeyword));
					if (keyword != null)
						card.Keywords.Add(keyword);
				}

				result.NumberOfCards = 1;
				result.Cards = new List<Card> { card };
			}
			else
			{
				using (BllCard bll = new BllCard(context))
				{
					var res = bll.GetCardsByUID(ScryFallRestClient.GetInstance(), cardUID);
					result.NumberOfCards = res.numberOfCards;
					result.Cards = res.cards;
				}
			}

			return result;
		}

		[Authorize]
		/// <summary>Gets all cards containing a specific string in it's name.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="name">String.</param>
		/// <param name="limit">Limit of card in request (no limit by default, -1).</param>
		/// <returns>List of cards.</returns>
		public async Task<CardReturnType> GetCardsByName([Service] MagicAppContext context, string name, int limit = -1)
		{
			var result = new CardReturnType(0, new List<Card>());

			using (BllCard bll = new BllCard(context))
			{
				var res = bll.GetCardsByName(ScryFallRestClient.GetInstance(), name, limit);
				result.NumberOfCards = res.numberOfCards;
				result.Cards = res.cards;
			}

			return result;
		}
	}
}
