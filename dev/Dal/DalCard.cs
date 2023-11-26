/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Context;
using MagicAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicAppAPI.Dal
{
	/// <summary>Data access layer for card objects.</summary>
	public class DalCard
	{
		#region Private Properties

		/// <summary>Database context.</summary>
		private MagicAppContext _appContext;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public DalCard(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		#endregion Constructor 

		#region Public Methods

		#region Getters

		/// <summary>Gets a dard by unique identifier.</summary>
		/// <param name="uid">Unique identifier.</param>
		/// <returns>A <see cref="Card"/> object (null if not found).</returns>
		public Card? GetByUID(string uid)
		{
			return _appContext.Cards.FirstOrDefault(c => c.UID == uid);
		}

		/// <summary>Gets a list of cards in the given collection.</summary>
		/// <param name="collectionId">Collection identifier.</param>
		/// <returns>Tuple representing the number of cards in the collection and the list of cards in the collection.</returns>
		public (long numberOfCards, List<Card> cards) GetByCollection(int collectionId)
		{
			(long numberOfCards, List<Card> cards) result = (0, new List<Card>());

			var collectionCards = _appContext.CollectionCards.Where(cc => cc.CollectionId == collectionId).ToList();
			Card? currentCard = null;
			foreach (var collectionCard in collectionCards)
			{
				currentCard = _appContext.Cards
					.Include(c => c.CardColors)
					.ThenInclude(cc => cc.Color)
					.Include(c => c.CardKeywords)
					.ThenInclude(ck => ck.Keyword)
					.Include(c => c.CardTypes)
					.ThenInclude(ct => ct.Type)
					.FirstOrDefault(c => c.Id == collectionCard.CardId);
				if (currentCard != null)
				{
					result.cards.Add(currentCard);
				}
			}
			result.numberOfCards = result.cards.Count;

			return result;
		}

		#endregion Getters

		#region Adding

		/// <summary>Adds a card in database and saves the context.</summary>
		/// <param name="card">Card to add.</param>
		public void AddCard(Card card)
		{
			_appContext.Cards.Add(card);
			_appContext.SaveChanges();
		}

		#endregion Adding

		#endregion Public Methods
	}
}
