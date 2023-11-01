/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.ExternalAPIs;
using MagicAppAPI.Models;

namespace MagicAppAPI.Bll
{
	public class BllCard : IDisposable
	{
		/// <summary>Gets cards given a set code and options.</summary>
		/// <param name="setCode">Set code.</param>
		/// <param name="includeExtras">Boolean indicating if the request should include extra cards.</param>
		/// <param name="includeVariations">Boolean indicating if the request should include variations cards.</param>
		/// <returns>Tuple representing the number of cards in the set and the list of cards in the set.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsBySetCode(IRestClient client, string setCode, bool includeExtras = true, bool includeVariations = true)
		{
			return client.GetCardsBySetCode(setCode, includeExtras, includeVariations);
		}

		/// <summary>Gets all cards given code in specific set.</summary>
		/// <param name="code">Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByCode(IRestClient client, string code, string setCode)
		{
			return client.GetCardsByCode(code, setCode);
		}

		/// <summary>Gets all cards given code in specific set.</summary>
		/// <param name="mtgCode">MTG Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByMTGCode(IRestClient client, string mtgCode, string setCode)
		{
			return client.GetCardsByMTGCode(mtgCode, setCode);
		}


		/// <summary>Gets all cards given card unique identifier.</summary>
		/// <param name="cardUID">Card unique identifier.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByUID(IRestClient client, string cardUID)
		{
			return client.GetCardsByUID(cardUID);
		}

		/// <summary>Gets all cards containing a specific string in it's name.</summary>
		/// <param name="name">String.</param>
		/// <param name="limit">Limit of card in request (no limit by default, -1).</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByName(IRestClient client, string name, int limit = -1)
		{
			return client.GetCardsByName(client, name, limit);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
