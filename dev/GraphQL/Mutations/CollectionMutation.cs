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
using MagicAppAPI.Enums;
using MagicAppAPI.ExternalAPIs.ScryFall;
using MagicAppAPI.GraphQL.ReturnTypes;
using MagicAppAPI.Models;
using MagicAppAPI.Tools;

namespace MagicAppAPI.GraphQL.Mutations
{
    /// <summary>Class that handles collection, including all additions, modifications and deletions.</summary>
    [ExtendObjectType("Mutation")]
	public class CollectionMutation
	{
		#region Public Methods

		#region Adding Decks

		/// <summary>Adds a deck to the current user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="name">Deck name.</param>
		/// <param name="description">Deck description.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionActionReturnType> AddDeckToCurrentUser([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, string name, string description)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new CollectionActionReturnType(404, "FAILURE: Current user not found.");

			using (var bllCollection = new BllCollection(context))
			{
				var decks = bllCollection.GetUserCollections(result.user.Id);
				if (decks.Any(deck => deck.Name == name))
					return new CollectionActionReturnType(403, "FAILURE: A deck with the same name already exists for this user.");

				bllCollection.AddCollectionToUser(result.user, name, description);
				return new CollectionActionReturnType(200, $"SUCCESS: Deck {name} created.");
			}
		}

		/// <summary>Adds a deck to the specific user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="name">Deck name.</param>
		/// <param name="description">Deck description.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionActionReturnType> AddDeckToUserByUserId([Service] MagicAppContext context, int userId, string name, string description)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionActionReturnType(404, "FAILURE: User not found.");

			using (var bllCollection = new BllCollection(context))
			{
				var decks = bllCollection.GetUserCollections(user.Id);
				if (decks.Any(deck => deck.Name == name))
					return new CollectionActionReturnType(403, "FAILURE: A deck with the same name already exists for this user.");

				bllCollection.AddCollectionToUser(user, name, description);
				return new CollectionActionReturnType(200, $"SUCCESS: Deck {name} created.");
			}
		}

		#endregion Adding Decks

		#region Editing Decks

		/// <summary>Adds a deck to the current user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="collectionId">Deck identifier.</param>
		/// <param name="name">Deck name.</param>
		/// <param name="description">Deck description.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionActionReturnType> ModifyCurrentUserDeck([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId, string name, string description)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new CollectionActionReturnType(404, "FAILURE: Current user not found.");

			using (var bllCollection = new BllCollection(context))
			{
				var deck = bllCollection.GetUserCollectionById(result.user.Id, collectionId);
				if (deck == null)
					return new CollectionActionReturnType(404, "FAILURE: Deck not found.");

				if (deck.Name == "MyCollection")
					return new CollectionActionReturnType(403, "FORBIDDEN: Unable to modify main collection.");

				if (name != deck.Name)
				{
					var decks = bllCollection.GetUserCollections(result.user.Id);
					if (decks.Any(deck => deck.Name == name))
						return new CollectionActionReturnType(403, "FAILURE: A deck with the same name already exists for this user.");
				}

				bllCollection.ModifyCollection(deck, name, description);
				return new CollectionActionReturnType(200, $"SUCCESS: Deck {name} modified.");
			}
		}

		/// <summary>Adds a deck to the specific user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="collectionId">Deck identifier.</param>
		/// <param name="name">Deck name.</param>
		/// <param name="description">Deck description.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionActionReturnType> ModifyUserDeckByUserId([Service] MagicAppContext context, int userId, int collectionId, string name, string description)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionActionReturnType(404, "FAILURE: User not found.");

			using (var bllCollection = new BllCollection(context))
			{
				var deck = bllCollection.GetUserCollectionById(user.Id, collectionId);
				if (deck == null)
					return new CollectionActionReturnType(404, "FAILURE: Deck not found.");

				if (deck.Name == "MyCollection")
					return new CollectionActionReturnType(403, "FORBIDDEN: Unable to modify main collection.");

				if (name != deck.Name)
				{
					var decks = bllCollection.GetUserCollections(user.Id);
					if (decks.Any(deck => deck.Name == name))
						return new CollectionActionReturnType(403, "FAILURE: A deck with the same name already exists for this user.");
				}

				bllCollection.ModifyCollection(deck, name, description);
				return new CollectionActionReturnType(200, $"SUCCESS: Deck {name} modified.");
			}
		}

		#endregion Editing Decks

		#region Deleting Decks

		/// <summary>Deletes a deck for the current user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="collectionId">Deck identifier.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionActionReturnType> DeleteCurrentUserDeck([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new CollectionActionReturnType(404, "FAILURE: Current user not found.");

			using (var bllCollection = new BllCollection(context))
			{
				var deck = bllCollection.GetUserCollectionById(result.user.Id, collectionId);
				if (deck == null)
					return new CollectionActionReturnType(404, "FAILURE: Deck not found.");

				if (deck.Name == "MyCollection")
					return new CollectionActionReturnType(403, "FORBIDDEN: Unable to delete main collection.");

				bllCollection.DeleteCollection(deck);
				return new CollectionActionReturnType(200, $"SUCCESS: Deck {deck.Name} deleted.");
			}
		}

		/// <summary>Deletes a deck for the specific user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="collectionId">Deck identifier.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionActionReturnType> DeleteUserDeckByUserId([Service] MagicAppContext context, int userId, int collectionId)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionActionReturnType(404, "FAILURE: User not found.");

			using (var bllCollection = new BllCollection(context))
			{
				var deck = bllCollection.GetUserCollectionById(user.Id, collectionId);
				if (deck == null)
					return new CollectionActionReturnType(404, "FAILURE: Deck not found.");

				if (deck.Name == "MyCollection")
					return new CollectionActionReturnType(403, "FORBIDDEN: Unable to delete main collection.");

				bllCollection.DeleteCollection(deck);
				return new CollectionActionReturnType(200, $"SUCCESS: Deck {deck.Name} deleted.");
			}
		}

		#endregion Deleting Decks

		#region Adding Cards

		/// <summary>Adds multiples cards to specific collection for current user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="collectionId">Collection's identifier.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to add in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to add in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to add in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to add in English language.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionActionReturnType> AddCardsToCurrentUserCollection([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new CollectionActionReturnType(404, "FAILURE: Current user not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == result.user.Id);
			if (collection is null)
				return new CollectionActionReturnType(404, "FAILURE: Collection not found.");

			return AddCardsToCollection(context, collection, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers);
		}

		/// <summary>Adds multiples cards to specific user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="collectionId">Collection's identifier.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to add in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to add in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to add in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to add in English language.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionActionReturnType> AddCardsToUserCollectionByUserId([Service] MagicAppContext context, int userId, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionActionReturnType(404, "FAILURE: User not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == user.Id);
			if (collection is null)
				return new CollectionActionReturnType(404, "FAILURE: Collection not found.");

			return AddCardsToCollection(context, collection, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers);
		}

		#endregion Adding Cards 

		#region Removing Cards

		/// <summary>Removes multiples cards from specific collection for current user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="collectionId">Collection's identifier.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to remove in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to remove in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to remove in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to remove in English language.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionActionReturnType> RemoveCardsFromCurrentUserCollection([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new CollectionActionReturnType(404, "FAILURE: Current user not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == result.user.Id);
			if (collection is null)
				return new CollectionActionReturnType(404, "FAILURE: Collection not found.");

			return RemoveCardsFromCollection(context, collection, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers);
		}

		/// <summary>Removes multiples cards from specific user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="collectionId">Collection's identifier.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to remove in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil remove to add in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to remove in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to remove in English language.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionActionReturnType> RemoveCardsFromUserCollectionByUserId([Service] MagicAppContext context, int userId, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionActionReturnType(404, "FAILURE: User not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == userId);
			if (collection is null)
				return new CollectionActionReturnType(404, "FAILURE: Collection not found.");

			return RemoveCardsFromCollection(context, collection, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers);
		}

		#endregion Removing Cards

		#endregion Public Methods

		#region Private Methods

		/// <summary>Internal methods adding a list of card to the given collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="collection">Collection to add card.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to add in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to add in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to add in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to add in English language.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		private CollectionActionReturnType AddCardsToCollection([Service] MagicAppContext context, Collection collection, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			CollectionActionReturnType results = new CollectionActionReturnType();

			int statusCode = 200;
			ERequestResult result;
			var nbErrors = 0;

			int frenchNumber;
			int frenchFoilNumber;
			int englishNumber;
			int englishFoilNumber;

			for (int i = 0; i < cardUIDs.Count; i++)
			{
				frenchNumber = frenchNumbers.ElementAtOrDefault(i);
				frenchFoilNumber = frenchFoilNumbers.ElementAtOrDefault(i);
				englishNumber = englishNumbers.ElementAtOrDefault(i);
				englishFoilNumber = englishFoilNumbers.ElementAtOrDefault(i);

				if (frenchNumber > 0 || frenchFoilNumber > 0 || englishNumber > 0 || englishFoilNumber > 0)
				{
					using (var bllCard = new BllCard(context))
					{
						var cardUID = cardUIDs.ElementAtOrDefault(i);
						if (cardUID != null)
							result = bllCard.AddCardToCollection(ScryFallRestClient.GetInstance(), collection, cardUID, frenchNumber, frenchFoilNumber, englishNumber, englishFoilNumber);
						else
							result = ERequestResult.INVALID_PARAMETERS;

						switch (result)
						{
							case ERequestResult.CARD_NOT_FOUND:
							case ERequestResult.COLLECTION_NOT_FOUND:
								statusCode = -1;
								results.AddResult(404, result.ToString(), cardUID ?? string.Empty);
								nbErrors++;
								break;
							case ERequestResult.CARD_ADDED:
								results.AddResult(200, result.ToString(), cardUID ?? string.Empty);
								break;
							default:
								results.AddResult(0, result.ToString(), cardUID ?? string.Empty);
								break;
						}
					}
				}
			}

			results.ChangeStatusCode(statusCode);
			results.ChangeMessage($"Cards added to collection {collection.Name} with {nbErrors} errro(s).");

			return results;
		}

		/// <summary>Internal methods removing a list of card from the given collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="user">User.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to remove in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to remove in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to remove in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to remove in English language.</param>
		/// <returns>A <see cref="CollectionActionReturnType"/> object containing result of the request.</returns>
		private CollectionActionReturnType RemoveCardsFromCollection([Service] MagicAppContext context, Collection collection, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			CollectionActionReturnType results = new CollectionActionReturnType();

			int statusCode = 200;
			ERequestResult result;
			var nbErrors = 0;

			int frenchNumber;
			int frenchFoilNumber;
			int englishNumber;
			int englishFoilNumber;

			for (int i = 0; i < cardUIDs.Count; i++)
			{
				frenchNumber = frenchNumbers.ElementAtOrDefault(i);
				frenchFoilNumber = frenchFoilNumbers.ElementAtOrDefault(i);
				englishNumber = englishNumbers.ElementAtOrDefault(i);
				englishFoilNumber = englishFoilNumbers.ElementAtOrDefault(i);

				if (frenchNumber > 0 || frenchFoilNumber > 0 || englishNumber > 0 || englishFoilNumber > 0)
				{
					using (var bllCard = new BllCard(context))
					{
						var cardUID = cardUIDs.ElementAtOrDefault(i);
						if (cardUID != null)
							result = bllCard.RemoveCardFromCollection(collection, cardUID, frenchNumber, frenchFoilNumber, englishNumber, englishFoilNumber);
						else
							result = ERequestResult.INVALID_PARAMETERS;

						switch (result)
						{
							case ERequestResult.CARD_NOT_FOUND:
							case ERequestResult.CARD_NOT_FOUND_IN_COLLECTION:
								statusCode = -1;
								results.AddResult(404, result.ToString(), cardUID ?? string.Empty);
								nbErrors++;
								break;
							case ERequestResult.CARD_REMOVED:
								results.AddResult(200, result.ToString(), cardUID ?? string.Empty);
								break;
							default:
								results.AddResult(0, result.ToString(), cardUID ?? string.Empty);
								break;
						}
					}
				}
			}

			results.ChangeStatusCode(statusCode);
			results.ChangeMessage($"Cards removed from collection with {nbErrors} errro(s).");

			return results;
		}

		#endregion Private Methods
	}
}
