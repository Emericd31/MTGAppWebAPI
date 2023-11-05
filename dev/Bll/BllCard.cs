/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Context;
using MagicAppAPI.Dal;
using MagicAppAPI.Enums;
using MagicAppAPI.ExternalAPIs;
using MagicAppAPI.Models;

namespace MagicAppAPI.Bll
{
	public class BllCard : IDisposable
	{
		#region Private Properties

		/// <summary>Data access layer for collection.</summary>
		private DalCollection _dalCollection;

		/// <summary>Data access layer for cards.</summary>
		private DalCard _dalCard;

		/// <summary>Data access layer for card colors.</summary>
		private DalColor _dalColor;

		/// <summary>Data access layer for card types.</summary>
		private DalType _dalType;

		/// <summary>Data access layer for card keywords.</summary>
		private DalKeyword _dalKeyword;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public BllCard(MagicAppContext appContext)
		{
			_dalCollection = new DalCollection(appContext);
			_dalCard = new DalCard(appContext);
			_dalColor = new DalColor(appContext);
			_dalType = new DalType(appContext);
			_dalKeyword = new DalKeyword(appContext);
		}

		#endregion Constructor

		#region Public Methods

		#region Getters

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

		#endregion Getters

		#region Adding

		/// <summary>Adds a card to the given collection by knowing the card unique identifier.</summary>
		/// <param name="client">External API that will be used to get the card if it doesn't exists in database.</param>
		/// <param name="collection">Cards collection.</param>
		/// <param name="collection">Cards collection.</param>
		/// <param name="cardUID">Card unique identifer.</param>
		/// <param name="frenchNumber">French number of cards to add.</param>
		/// <param name="frenchFoilNumber">French number of foil cards to add.</param>
		/// <param name="englishNumber">English number of cards to add.</param>
		/// <param name="englishFoilNumber">English number of foil cards to add.</param>
		/// <returns>The <see cref="ERequestResult"/> result.</returns>
		public ERequestResult AddCardToCollection(IRestClient client, Collection collection, string cardUID, int frenchNumber, int frenchFoilNumber, int englishNumber, int englishFoilNumber)
		{
			if (collection == null)
				return ERequestResult.COLLECTION_NOT_FOUND;

			var card = _dalCollection.GetByUID(cardUID);
			if (card == null)
			{
				var res = GetCardsByUID(client, cardUID);
				if (res.numberOfCards == 0)
					return ERequestResult.CARD_NOT_FOUND;

				card = res.cards.First();
				_dalCollection.AddCard(card);

				_dalColor.AddCardColors(card, card.Colors);
				_dalType.AddCardTypes(card, card.Types);
				_dalKeyword.AddCardKeywords(card, card.Keywords);
			}

			var collectionCard = _dalCollection.GetCollectionCard(collection.Id, card.Id);
			if (collectionCard == null)
				_dalCollection.AddCollectionCard(collection, card, frenchNumber, frenchFoilNumber, englishNumber, englishFoilNumber);
			else
				_dalCollection.ModifyCollectionCard(collectionCard, frenchNumber, frenchFoilNumber, englishNumber, englishFoilNumber);

			var nbCardsInCollection = collection.NbCards + frenchNumber + frenchFoilNumber + englishNumber + englishFoilNumber;
			_dalCollection.ModifyNbCardCollection(collection, nbCardsInCollection);

			return ERequestResult.CARD_ADDED;
		}

		#endregion Adding

		#region Removing

		/// <summary>Removes a card from the given collection by knowing the card unique identifier.</summary>
		/// <param name="collection">Cards collection.</param>
		/// <param name="cardUID">Card unique identifer.</param>
		/// <param name="frenchNumber">French number of cards to remove.</param>
		/// <param name="frenchFoilNumber">French number of foil cards to remove.</param>
		/// <param name="englishNumber">English number of cards to remove.</param>
		/// <param name="englishFoilNumber">English number of foil cards to remove.</param>
		/// <returns>The <see cref="ERequestResult"/> result.</returns>
		public ERequestResult RemoveCardFromCollection(Collection collection, string cardUID, int frenchNumber, int frenchFoilNumber, int englishNumber, int englishFoilNumber)
		{
			var card = _dalCard.GetByUID(cardUID);
			if (card == null)
				return ERequestResult.CARD_NOT_FOUND;

			var collectionCard = _dalCollection.GetCollectionCard(collection.Id, card.Id);
			if (collection == null)
				return ERequestResult.CARD_NOT_FOUND_IN_COLLECTION;

			var newFrenchNumber = collectionCard.FrenchNumber - frenchNumber;
			var newFrenchFoilNumber = collectionCard.FrenchFoilNumber - frenchFoilNumber;
			var newEnglishNumber = collectionCard.EnglishNumber - englishNumber;
			var newEnglishFoilNumber = collectionCard.EnglishFoilNumber - englishFoilNumber;

			collection.NbCards -= (newFrenchNumber < 0) ? collectionCard.FrenchNumber : frenchNumber;
			collection.NbCards -= (newFrenchFoilNumber < 0) ? collectionCard.FrenchFoilNumber : frenchFoilNumber;
			collection.NbCards -= (newEnglishNumber < 0) ? collectionCard.EnglishNumber : englishNumber;
			collection.NbCards -= (newEnglishFoilNumber < 0) ? collectionCard.EnglishFoilNumber : englishFoilNumber;

			_dalCollection.ModifyNbCardCollection(collection);

			collectionCard.FrenchNumber = newFrenchNumber >= 0 ? newFrenchNumber : 0;
			collectionCard.FrenchFoilNumber = newFrenchFoilNumber >= 0 ? newFrenchFoilNumber : 0;
			collectionCard.EnglishNumber = newEnglishNumber >= 0 ? newEnglishNumber : 0;
			collectionCard.EnglishFoilNumber = newEnglishFoilNumber >= 0 ? newEnglishFoilNumber : 0;

			_dalCollection.ModifyCollectionCard(collectionCard);

			if (collectionCard.FrenchNumber == 0 && collectionCard.FrenchFoilNumber == 0
				&& collectionCard.EnglishNumber == 0 && collectionCard.EnglishFoilNumber == 0)
				_dalCollection.RemoveCollectionCard(collectionCard);

			return ERequestResult.CARD_REMOVED;
		}

		#endregion Removing

		/// <summary>Dispose methods.</summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		#endregion Public Methods
	}
}
