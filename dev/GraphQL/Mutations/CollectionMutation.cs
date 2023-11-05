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
using MagicAppAPI.GraphQL.Mutations.ReturnTypes;
using MagicAppAPI.Models;
using System.Security.Claims;

namespace MagicAppAPI.GraphQL.Mutations
{
	/// <summary>Class that handles collection, including all additions, modifications and deletions.</summary>
	[ExtendObjectType("Mutation")]
	public class CollectionMutation
	{
		#region Public Methods

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
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionReturnType> AddCardsToCurrentUserCollection([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			if (httpContextAccessor == null)
				return new CollectionReturnType(100, "");

			int currentUserId = -1;
			if (int.TryParse(httpContextAccessor?.HttpContext?.User.FindFirstValue("userId"), out int receivedId))
				currentUserId = receivedId;

			var currentUser = await context.Users.FindAsync(currentUserId);
			if (currentUser is null)
				return new CollectionReturnType(404, "FAILURE: Current user not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == currentUserId);
			if (collection is null)
				return new CollectionReturnType(404, "FAILURE: Collection not found.");

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
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionReturnType> AddCardsToUserCollectionByUserId([Service] MagicAppContext context, int userId, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionReturnType(404, "FAILURE: User not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == user.Id);
			if (collection is null)
				return new CollectionReturnType(404, "FAILURE: Collection not found.");

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
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionReturnType> RemoveCardsFromCurrentUserCollection([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			int currentUserId = -1;
			if (int.TryParse(httpContextAccessor?.HttpContext?.User.FindFirstValue("userId"), out int receivedId))
				currentUserId = receivedId;
			var currentUser = await context.Users.FindAsync(currentUserId);
			if (currentUser is null)
				return new CollectionReturnType(404, "FAILURE: Current user not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == currentUserId);
			if (collection is null)
				return new CollectionReturnType(404, "FAILURE: Collection not found.");

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
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionReturnType> RemoveCardsFromUserCollectionByUserId([Service] MagicAppContext context, int userId, int collectionId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionReturnType(404, "FAILURE: User not found.");

			var collection = context.Collections.FirstOrDefault(c => c.Id == collectionId && c.UserId == userId);
			if (collection is null)
				return new CollectionReturnType(404, "FAILURE: Collection not found.");

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
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		private CollectionReturnType AddCardsToCollection([Service] MagicAppContext context, Collection collection, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			CollectionReturnType results = new CollectionReturnType();

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

			results.AddStatusCode(statusCode);
			results.AddMessage($"Cards added to collection {collection.Name} with {nbErrors} errro(s).");

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
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		private CollectionReturnType RemoveCardsFromCollection([Service] MagicAppContext context, Collection collection, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			CollectionReturnType results = new CollectionReturnType();

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

			results.AddStatusCode(statusCode);
			results.AddMessage($"Cards removed from collection with {nbErrors} errro(s).");

			return results;
		}

		#endregion Private Methods
	}
}
