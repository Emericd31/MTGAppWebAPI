﻿/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
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
	/// <summary>Data access layer for collection/collectionCard objects.</summary>
	public class DalCollection
	{
		#region Private Properties

		/// <summary>Database context.</summary>
		private MagicAppContext _appContext;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public DalCollection(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		#endregion Constructor

		#region Public Methods

		#region Getters

		/// <summary>Gets the list of collection for the specific user.</summary>
		/// <param name="userId">User identifier.</param>
		/// <returns>The list of <see cref="Collection"/>.</returns>
		public IQueryable<Collection> GetUserCollections(int userId)
		{
			return _appContext.Collections.Where(x => x.UserId == userId);
		}

		/// <summary>Gets the specific collection for user.</summary>
		/// <param name="userId">User identifier.</param>
		/// <param name="collectionId">Collection identifier.</param>
		/// <returns>The <see cref="Collection"/> (null if not found).</returns>
		public Collection? GetUserCollectionById(int userId, int collectionId)
		{
			return _appContext.Collections.FirstOrDefault(x => x.UserId == userId && x.Id == collectionId);
		}

		/// <summary>Gets all cards present in database.</summary>
		/// <returns>The list of <see cref="Card"/>.</returns>
		public IQueryable<Card> GetAllCards()
		{
			return _appContext.Cards;
		}

		/// <summary>Gets a card by knowing its unique identifier.</summary>
		/// <param name="cardUID">Card unique identifier.</param>
		/// <returns>The <see cref="Card"/> (null if not found).</returns>
		public Card? GetByUID(string cardUID)
		{
			return GetAllCards().FirstOrDefault(card => card.UID == cardUID);
		}

		/// <summary>Gets the collection cards object by knowing a collection identifier and a card identifier.</summary>
		/// <param name="collectionId">Database identifier for collection.</param>
		/// <param name="cardId">Database identifier for card.</param>
		/// <returns>The <see cref="CollectionCards"/> (null if not found).</returns>
		public CollectionCards? GetCollectionCard(int collectionId, int cardId)
		{
			return _appContext.CollectionCards.FirstOrDefault(element => element.CollectionId == collectionId && element.CardId == cardId);
		}

		#endregion Getters

		#region Adding

		/// <summary>Adds a collection in database and saves the context.</summary>
		/// <param name="collection">Collection to add</param>
		public void AddCollection(Collection collection)
		{
			_appContext.Collections.Add(collection);
			_appContext.SaveChanges();
		}

		/// <summary>Adds a collection cards object in database and saves the context.</summary>
		/// <param name="collection">Collection.</param>
		/// <param name="card">Card.</param>
		/// <param name="frenchNumber">French number of cards.</param>
		/// <param name="frenchFoilNumber">French number of foil cards.</param>
		/// <param name="englishNumber">English number of cards.</param>
		/// <param name="englishFoilNumber">English number of foil cards.</param>
		public void AddCollectionCard(Collection collection, Card card, int frenchNumber, int frenchFoilNumber, int englishNumber, int englishFoilNumber)
		{
			var newCollectionCard = new CollectionCards
			{
				Collection = collection,
				CollectionId = collection.Id,
				Card = card,
				CardId = card.Id,
				FrenchNumber = frenchNumber,
				FrenchFoilNumber = frenchFoilNumber,
				EnglishNumber = englishNumber,
				EnglishFoilNumber = englishFoilNumber
			};

			_appContext.CollectionCards.Add(newCollectionCard);
			_appContext.SaveChanges();
		}

		#endregion Adding

		#region Setters

		/// <summary>Modifies the collection information.</summary>
		/// <param name="collection">Collection.</param>
		/// <param name="name">Name.</param>
		/// <param name="description">Descritpion.</param>
		public void ModifyCollection(Collection collection, string name, string description)
		{
			collection.Name = name;
			collection.Description = description;

			_appContext.Attach(collection);
			_appContext.Entry(collection).Property("Name").IsModified = true;
			_appContext.Entry(collection).Property("Description").IsModified = true;
			_appContext.SaveChanges();
		}

		/// <summary>Modifies the number of cards in the given collection and saves the context.</summary>
		/// <param name="collection">Collection.</param>
		/// <param name="nbCard">Number of card in collection (-1 will just save the current number of cards in collection in database).</param>
		public void ModifyNbCardCollection(Collection collection, int nbCard = -1)
		{
			if (nbCard >= 0)
				collection.NbCards = nbCard;

			_appContext.Attach(collection);
			_appContext.Entry(collection).Property("NbCards").IsModified = true;
			_appContext.SaveChanges();
		}

		/// <summary>Modifies the price of the collection.</summary>
		/// <param name="collection">Collection.</param>
		/// <param name="usdPrice">New USD price.</param>
		/// <param name="eurPrice">New EUR price.</param>
		public void ModifyCollectionPrices(Collection collection, float usdPrice, float eurPrice)
		{
			if (usdPrice < 0 || eurPrice < 0)
				return;

			collection.USDPrice = usdPrice;
			collection.EURPrice = eurPrice;
		}

		/// <summary>Modifies the number of cards in collection cards object and saves the context.</summary>
		/// <param name="collectionCard">Collection cards object.</param>
		/// <param name="frenchNumber">French number of cards to add (can be negative).</param>
		/// <param name="frenchFoilNumber">French number of foil cards add (can be negative)</param>
		/// <param name="englishNumber">English number of cards add (can be negative)</param>
		/// <param name="englishFoilNumber">English number of foil cards add (can be negative)</param>
		public void ModifyCollectionCard(CollectionCards collectionCard, int frenchNumber = 0, int frenchFoilNumber = 0, int englishNumber = 0, int englishFoilNumber = 0)
		{
			collectionCard.FrenchNumber += frenchNumber;
			collectionCard.FrenchFoilNumber += frenchFoilNumber;
			collectionCard.EnglishNumber += englishNumber;
			collectionCard.EnglishFoilNumber += englishFoilNumber;

			_appContext.Entry(collectionCard).State = EntityState.Modified;
			_appContext.SaveChanges();
		}

		#endregion Setters

		#region Removing

		/// <summary>Deletes a specific collection.</summary>
		/// <param name="collection">Collection.</param>
		public void DeleteCollection(Collection collection)
		{
			_appContext.Remove(collection);
			_appContext.SaveChanges();
		}

		/// <summary>Removes a collection cards object and saves the context.</summary>
		/// <param name="collectionCard">Collection cards object.</param>
		public void RemoveCollectionCard(CollectionCards collectionCard)
		{
			_appContext.Remove(collectionCard);
			_appContext.SaveChanges();
		}

		#endregion Removing

		#endregion Public Methods
	}
}
