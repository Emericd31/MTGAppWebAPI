/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.Text.Json;
using MagicAppAPI.Models;
using MagicAppAPI.Tools;

namespace MagicAppAPI.ExternalAPIs.ScryFall
{
	public sealed class ScryFallRestClient : IRestClient
	{
		private ScryFallRestClient() { }

		private static ScryFallRestClient _instance;

		// We now have a lock object that will be used to synchronize threads
		// during first access to the Singleton.
		private static readonly object _lock = new object();

		#region API links

		private const string SET_BASE_API_ROUTE = "https://api.scryfall.com/sets/";

		private const string CARD_BASE_API_ROUTE = "https://api.scryfall.com/cards/";

		#endregion API links

		#region Private Properties

		private string CARD_SETS_API_ROUTE = CARD_BASE_API_ROUTE + "search?include_extras={0}&order=set&q=e%3A{1}{2}";

		private string CARD_BY_CODE_API_ROUTE = CARD_BASE_API_ROUTE + "search?q=number%3A{0}+set%3A{1}";

		private string CARD_BY_MTG_CODE_API_ROUTE = CARD_BASE_API_ROUTE + "{0}/{1}";

		private string CARD_BY_UID_API_ROUTE = CARD_BASE_API_ROUTE + "{0}";

		private string CARD_BY_NAME_API_ROUTE = CARD_BASE_API_ROUTE + "search?q={0}&unique=prints";

		private HttpClient httpClient { get; set; } = new HttpClient();

		#endregion Private Properties

		public static ScryFallRestClient GetInstance()
		{
			// This conditional is needed to prevent threads stumbling over the
			// lock once the instance is ready.
			if (_instance == null)
			{
				// Now, imagine that the program has just been launched. Since
				// there's no Singleton instance yet, multiple threads can
				// simultaneously pass the previous conditional and reach this
				// point almost at the same time. The first of them will acquire
				// lock and will proceed further, while the rest will wait here.
				lock (_lock)
				{
					// The first thread to acquire the lock, reaches this
					// conditional, goes inside and creates the Singleton
					// instance. Once it leaves the lock block, a thread that
					// might have been waiting for the lock release may then
					// enter this section. But since the Singleton field is
					// already initialized, the thread won't create a new
					// object.
					if (_instance == null)
					{
						_instance = new ScryFallRestClient();
					}
				}
			}
			return _instance;
		}

		#region Sets

		/// <summary>Gets all available sets.</summary>
		/// <returns>List of available sets.</returns>
		public List<Set> GetSets()
		{
			List<Set> setList = new List<Set>();

			var uri = new Uri(SET_BASE_API_ROUTE);

			var result = "";

			try
			{
				result = httpClient.GetStringAsync(uri).Result;
			}
			catch
			{
				return new List<Set>();
			}

			using (var unknownObject = JsonDocument.Parse(result))
			{
				using (var sets = unknownObject.RootElement.GetProperty("data").EnumerateArray())
				{
					while (sets.MoveNext())
					{
						setList.Add(ScryFallDataConverters.SetConverter(sets.Current));
					}
				}
			}

			return setList;
		}

		/// <summary>Gets a set given a code.</summary>
		/// <param name="setCode">Set code.</param>
		/// <returns>Set object.</returns>
		public Set GetSetByCode(string setCode)
		{
			try
			{
				var uri = new Uri(SET_BASE_API_ROUTE + setCode);
				var result = httpClient.GetStringAsync(uri).Result;

				using (var unknownObject = JsonDocument.Parse(result))
				{
					return ScryFallDataConverters.SetConverter(unknownObject.RootElement);
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		#endregion Sets

		#region Cards

		/// <summary>Gets cards given a set code and options.</summary>
		/// <param name="setCode">Set code.</param>
		/// <param name="includeExtras">Boolean indicating if the request should include extra cards.</param>
		/// <param name="includeVariations">Boolean indicating if the request should include variations cards.</param>
		/// <returns>Tuple representing the number of cards in the set and the list of cards in the set.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsBySetCode(string setCode, bool includeExtras, bool includeVariations)
		{
			return Task.Run(async () =>
			{
				var cards = new List<Card>();
				long totalCards = 0;

				var url = string.Format(CARD_SETS_API_ROUTE, includeExtras.ToString().ToLower(), setCode, includeVariations ? "&unique=prints" : "");
				bool hasMore = true;
				while (hasMore)
				{
					(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url).ConfigureAwait(false);
					hasMore = list.hasMoreResult;
					url = list.urlResult;
					totalCards = list.totalCards;
					cards.AddRange(list.cards);
				}

				return (totalCards, cards);
			}).Result;
		}

		/// <summary>Gets all cards given code in specific set.</summary>
		/// <param name="code">Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByCode(string code, string setCode)
		{
			return Task.Run(async () =>
			{
				var cards = new List<Card>();
				long totalCards = 0;

				var url = string.Format(CARD_BY_CODE_API_ROUTE, code.ToLower(), setCode.ToLower());
				bool hasMore = true;
				while (hasMore)
				{
					(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url).ConfigureAwait(false);
					hasMore = list.hasMoreResult;
					url = list.urlResult;
					totalCards = list.totalCards;
					cards.AddRange(list.cards);
				}

				return (totalCards, cards);
			}).Result;
		}

		/// <summary>Gets all cards given mtg code in specific set.</summary>
		/// <param name="mtgCode">MTG card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByMTGCode(string mtgCode, string setCode)
		{
			return Task.Run(async () =>
			{
				var cards = new List<Card>();
				long totalCards = 0;

				var url = string.Format(CARD_BY_MTG_CODE_API_ROUTE, setCode.ToLower(), mtgCode.ToLower());
				bool hasMore = true;
				while (hasMore)
				{
					(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url, hasDataNode: false).ConfigureAwait(false);
					hasMore = list.hasMoreResult;
					url = list.urlResult;
					totalCards = list.totalCards;
					cards.AddRange(list.cards);
				}

				return (totalCards, cards);
			}).Result;
		}

		/// <summary>Gets all cards given card unique identifier.</summary>
		/// <param name="cardUID">Card unique identifier.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByUID(string cardUID)
		{
			return Task.Run(async () =>
			{
				var cards = new List<Card>();

				var url = string.Format(CARD_BY_UID_API_ROUTE, cardUID.ToLower());
				bool hasMore = true;
				while (hasMore)
				{
					(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url, hasDataNode: false).ConfigureAwait(false);
					hasMore = list.hasMoreResult;
					url = list.urlResult;
					cards.AddRange(list.cards);
				}

				return (cards.Count, cards);
			}).Result;
		}

		/// <summary>Gets all cards containing a specific string in it's name.</summary>
		/// <param name="name">String.</param>
		/// <param name="limit">Limit of card in request (no limit by default, -1).</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByName(IRestClient client, string name, int limit = -1)
		{
			return Task.Run(async () =>
			{
				var cards = new List<Card>();
				long totalCards = 0;

				var url = string.Format(CARD_BY_NAME_API_ROUTE, name.ToLower());
				bool hasMore = true;
				while (hasMore)
				{
					(bool hasMoreResult, string urlResult, List<Card> cards, long totalCards) list = await GetCardsByURL(url, limit: limit).ConfigureAwait(false);
					hasMore = list.hasMoreResult;
					url = list.urlResult;
					totalCards = list.totalCards;
					cards.AddRange(list.cards);
				}

				return (totalCards, cards);
			}).Result;
		}

		/// <summary>Gets cards by URL.</summary>
		/// <param name="url">URL.</param>
		/// <param name="limit">Limit of cards in the request, no limit by default (-1).</param>
		/// <param name="hasDataNode">Boolean indicating if the JSON result has data node or not (if not, root node is used).</param>
		/// <returns>Tuple containing  : 
		///		- A boolean indicating if there are more cards to gets.
		///		- The uri of the next part of the request.
		///		- The list of cards in this part of the request.
		///		- Total number of cards in the set.
		/// </returns>
		private async Task<(bool hasMoreResult, string urlResult, List<Card>, long totalCards)> GetCardsByURL(string url, int limit = -1, bool hasDataNode = true)
		{
			return Task.Run(async () =>
			{
				var cardsResult = new List<Card>();

				var uri = new Uri(url);

				var requestResult = "";

				try
				{
					requestResult = httpClient.GetStringAsync(uri).Result;
				}
				catch
				{
					return (hasMoreResult: false, uriResult: "", cardsResult, 0);
				}

				long totalCards = 0;
				bool hasMore = false;
				var nextPage = "";

				using (var unknownObject = JsonDocument.Parse(requestResult))
				{
					// Gets total number of cards in set
					JsonElement currentElement = new JsonElement();
					if (unknownObject.RootElement.TryGetProperty("total_cards", out currentElement))
						totalCards = JsonReader.TryGetLongFromJsonElement(currentElement);

					// Gets the value indicating if there are more cards to get
					if (limit > 0 && totalCards > limit)
					{
						hasMore = false;
					}
					else
					{
						if (unknownObject.RootElement.TryGetProperty("has_more", out currentElement))
						{
							hasMore = JsonReader.TryGetBoolFromJsonElement(currentElement);
						}
						else
						{
							// Do nothing
						}
					}

					// Gets the next page if has more
					if (hasMore)
					{
						if (unknownObject.RootElement.TryGetProperty("next_page", out currentElement))
							nextPage = JsonReader.TryGetStringFromJsonElement(currentElement);
					}
					else
					{
						// Do nothing
					}

					// Converts card into objects
					if (hasDataNode)
					{
						using (var cards = unknownObject.RootElement.GetProperty("data").EnumerateArray())
						{
							while (cards.MoveNext())
							{
								if (limit == 0)
									break;
								else if (limit > 0)
									limit--;
								cardsResult.Add(ScryFallDataConverters.CardConverter(cards.Current));
							}
						}
					}
					else
					{
						cardsResult.Add(ScryFallDataConverters.CardConverter(unknownObject.RootElement));
					}
				}

				return (hasMoreResult: hasMore, uriResult: nextPage, cardsResult, totalCards);
			}).Result;
		}

		#endregion Cards
	}
}
