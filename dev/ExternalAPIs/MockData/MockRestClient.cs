/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Models;
using MagicAppAPI.Models.Builders;

namespace MagicAppAPI.ExternalAPIs.MockData
{
	/// <summary>Class that implements API methods with mocked data (for test purpose).</summary>
	public sealed class MockRestClient : IRestClient
	{
		#region Private Properties

		/// <summary>Single instance of <see cref="MockRestClient"/>.</summary>
		private static MockRestClient? _instance;

		// We now have a lock object that will be used to synchronize threads
		// during first access to the Singleton.
		private static readonly object _lock = new object();

		#endregion Private Properties

		#region Constructor 

		/// <summary>Private constructor.</summary>
		private MockRestClient() { }

		#endregion Constructor

		#region Public Methods

		/// <summary>Gets the unique instance of Mock rest client (creates it if it doesn't exists).</summary>
		/// <returns>The <see cref="MockRestClient"/> object.</returns>
		public static MockRestClient GetInstance()
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
						_instance = new MockRestClient();
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
			return new List<Set>
			{
				new SetBuilder().Build(),
				new SetBuilder().Build()
			};
		}

		/// <summary>Gets a set given a code.</summary>
		/// <param name="setCode">Set code.</param>
		/// <returns>Set object.</returns>
		public Set GetSetByCode(string setCode)
		{
			return new SetBuilder().AddCode(setCode).Build();
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
			return (2, new List<Card>
			{
				new CardBuilder().AddName("Card1").AddSetCode(setCode).Build(),
				new CardBuilder().AddName("Card2").AddSetCode(setCode).Build()
			});
		}

		/// <summary>Gets all cards given code in specific set.</summary>
		/// <param name="code">Card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByCode(string code, string setCode)
		{
			return (1, new List<Card>
			{
				new CardBuilder().AddSetCode(setCode).Build()
			});
		}

		/// <summary>Gets all cards given mtg code in specific set.</summary>
		/// <param name="mtgCode">MTG card code.</param>
		/// <param name="setCode">Set code.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByMTGCode(string mtgCode, string setCode)
		{
			return (1, new List<Card>
			{
				new CardBuilder().AddSetCode(setCode).Build()
			});
		}

		/// <summary>Gets all cards given card unique identifier.</summary>
		/// <param name="cardUID">Card unique identifier.</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByUID(string cardUID)
		{
			return (1, new List<Card>
			{
				new CardBuilder().AddUID(cardUID).Build()
			});
		}

		/// <summary>Gets all cards containing a specific string in it's name.</summary>
		/// <param name="name">String.</param>
		/// <param name="limit">Limit of card in request (no limit by default, -1).</param>
		/// <returns>List of cards.</returns>
		public (long numberOfCards, List<Card> cards) GetCardsByName(IRestClient client, string name, int limit = -1)
		{
			return (2, new List<Card>
			{
				new CardBuilder().AddName(name + " card1").Build(),
				new CardBuilder().AddName(name + " card2").Build()
			});
		}

		#endregion Cards

		#endregion Public Methods
	}
}
