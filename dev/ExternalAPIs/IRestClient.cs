/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Models;

namespace MagicAppAPI.ExternalAPIs
{
	public interface IRestClient
	{
		/// <summary>Gets all available sets.</summary>
		/// <returns>List of available sets.</returns>
		public List<Set> GetSets();

		/// <summary>Gets a set given a code.</summary>
		/// <param name="setCode">Set code.</param>
		/// <returns>Set object.</returns>
		public Set GetSetByCode(string setCode);

		/// <summary>Gets cards given a set code and options.</summary>
		/// <param name="setCode">Set code.</param>
		/// <param name="includeExtras">Boolean indicating if the request should include extra cards.</param>
		/// <param name="includeVariations">Boolean indicating if the request should include variations cards.</param>
		/// <returns>Tuple representing the number of cards in the set and the list of cards in the set.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsBySetCode(string setCode, bool includeExtras, bool includeVariations);

		/// <summary>Gets all cards given code in specific set.</summary>
		/// <param name="code">Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByCode(string code, string setCode);

		/// <summary>Gets all cards given mtg code in specific set.</summary>
		/// <param name="mtgCode">MTG card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByMTGCode(string mtgCode, string setCode);

		/// <summary>Gets all cards given card unique identifier.</summary>
		/// <param name="cardUID">Card unique identifier.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByUID(string cardUID);

		/// <summary>Gets all cards containing a specific string in it's name.</summary>
		/// <param name="name">String.</param>
		/// <param name="limit">Limit of card in request (no limit, -1).</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByName(IRestClient client, string name, int limit);
	}
}
