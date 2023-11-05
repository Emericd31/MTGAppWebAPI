/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.Data;
using System.Security.Claims;
using HotChocolate.Authorization;
using MagicAppAPI.Bll;
using MagicAppAPI.Context;
using MagicAppAPI.Enums;
using MagicAppAPI.ExternalAPIs.ScryFall;
using MagicAppAPI.GraphQL.Mutations.ReturnTypes;
using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL
{
	[ExtendObjectType("Mutation")]
	public class CollectionMutationOld
	{
		#region Public Methods

		#region Adding Cards

		/// <summary>Adds multiples cards to current user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to add in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to add in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to add in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to add in English language.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionReturnType> AddCardsToCurrentUserCollection([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			int currentUserId = Int32.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("userId"));
			var currentUser = await context.Users.FindAsync(currentUserId);
			if (currentUser is null)
				return new CollectionReturnType(404, "FAILURE: Current user not found.");

			return await AddCardsToUserCollection(context, currentUser, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers).ConfigureAwait(false);
		}

		/// <summary>Adds multiples cards to current user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to add in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to add in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to add in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to add in English language.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionReturnType> AddCardsToUserCollectionByUserId([Service] MagicAppContext context, int userId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionReturnType(404, "FAILURE: User not found.");

			return await AddCardsToUserCollection(context, user, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers).ConfigureAwait(false);
		}

		#endregion Adding Cards

		#region Removing Cards

		/// <summary>Removes multiples cards from current user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to remove in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to remove in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to remove in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to remove in English language.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionReturnType> RemoveCardsFromCurrentUserCollection([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			int currentUserId = Int32.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("userId"));
			var currentUser = await context.Users.FindAsync(currentUserId);
			if (currentUser is null)
				return new CollectionReturnType(404, "FAILURE: Current user not found.");

			return await RemoveCardsFromUserCollection(context, currentUser, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers).ConfigureAwait(false);
		}


		/// <summary>Removes multiples cards from current user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to remove in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to remove in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to remove in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to remove in English language.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionReturnType> RemoveCardsFromUserCollectionByUserId([Service] MagicAppContext context, int userId, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionReturnType(404, "FAILURE: User not found.");

			return await RemoveCardsFromUserCollection(context, user, cardUIDs, frenchNumbers, frenchFoilNumbers, englishNumbers, englishFoilNumbers).ConfigureAwait(false);
		}

		#endregion Remove Cards

		#endregion Public Methods

		#region Private Methods

		/// <summary>Internal methods adding a list of card to the given user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="user">User.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to add in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to add in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to add in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to add in English language.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		private async Task<CollectionReturnType> AddCardsToUserCollection([Service] MagicAppContext context, User user, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			CollectionReturnType results = new CollectionReturnType();

			int statusCode = 200;
			ERequestResult result;
			var nbErrors = 0;

			results.AddStatusCode(statusCode);
			results.AddMessage($"Cards added to collection with {nbErrors} errro(s).");

			return results;

			//int frenchNumber;
			//int frenchFoilNumber;
			//int englishNumber;
			//int englishFoilNumber;

			//for (int i = 0; i < cardUIDs.Count; i++)
			//{
			//	frenchNumber = frenchNumbers.ElementAtOrDefault(i);
			//	frenchFoilNumber = frenchFoilNumbers.ElementAtOrDefault(i);
			//	englishNumber = englishNumbers.ElementAtOrDefault(i);
			//	englishFoilNumber = englishFoilNumbers.ElementAtOrDefault(i);

			//	if (frenchNumber > 0 || frenchFoilNumber > 0 || englishNumber > 0 || englishFoilNumber > 0)
			//	{
			//		using (var bllCard = new BllCard())
			//		{
			//			var cardUID = cardUIDs.ElementAtOrDefault(i);
			//			if (cardUID != null)
			//				result = await bllCard.AddCardToCollection(context, ScryFallRestClient.GetInstance(), user, cardUID, frenchNumber, frenchFoilNumber, englishNumber, englishFoilNumber).ConfigureAwait(false);
			//			else
			//				result = ERequestResult.INVALID_PARAMETERS;

			//			switch (result)
			//			{
			//				case ERequestResult.CARD_NOT_FOUND:
			//					statusCode = -1;
			//					results.AddResult(404, result.ToString(), cardUID);
			//					nbErrors++;
			//					break;
			//				case ERequestResult.CARD_ADDED:
			//					results.AddResult(200, result.ToString(), cardUID);
			//					break;
			//				default:
			//					results.AddResult(0, result.ToString(), cardUID);
			//					break;
			//			}
			//		}
			//	}
			//}

			//results.AddStatusCode(statusCode);
			//results.AddMessage($"Cards added to collection with {nbErrors} errro(s).");

			//return results;
		}

		/// <summary>Internal methods removing a list of card from the given user collection.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="user">User.</param>
		/// <param name="cardUIDs">List of card unique identifiers.</param>
		/// <param name="frenchNumbers">List of numbers of cards to remove in French language.</param>
		/// <param name="frenchFoilNumbers">List of numbers of foil cards to remove in French language.</param>
		/// <param name="englishNumbers">List of numbers of cards to remove in English language.</param>
		/// <param name="englishFoilNumbers">List of numbers of foil cards to remove in English language.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		private async Task<CollectionReturnType> RemoveCardsFromUserCollection([Service] MagicAppContext context, User user, List<string> cardUIDs, List<int> frenchNumbers, List<int> frenchFoilNumbers, List<int> englishNumbers, List<int> englishFoilNumbers)
		{
			CollectionReturnType results = new CollectionReturnType();

			int statusCode = 200;
			ERequestResult result;
			var nbErrors = 0;

			int frenchNumber;
			int frenchFoilNumber;
			int englishNumber;
			int englishFoilNumber;

			results.AddStatusCode(statusCode);
			results.AddMessage($"Cards removed from collection with {nbErrors} errro(s).");

			return results;

			//for (int i = 0; i < cardUIDs.Count; i++)
			//{
			//	frenchNumber = frenchNumbers.ElementAtOrDefault(i);
			//	frenchFoilNumber = frenchFoilNumbers.ElementAtOrDefault(i);
			//	englishNumber = englishNumbers.ElementAtOrDefault(i);
			//	englishFoilNumber = englishFoilNumbers.ElementAtOrDefault(i);

			//	if (frenchNumber > 0 || frenchFoilNumber > 0 || englishNumber > 0 || englishFoilNumber > 0)
			//	{
			//		using (var bllCard = new BllCard())
			//		{
			//			var cardUID = cardUIDs.ElementAtOrDefault(i);
			//			if (cardUID != null)
			//				result = await bllCard.RemoveCardFromCollection(context, user, cardUID, frenchNumber, frenchFoilNumber, englishNumber, englishFoilNumber).ConfigureAwait(false);
			//			else
			//				result = ERequestResult.INVALID_PARAMETERS;

			//			switch (result)
			//			{
			//				case ERequestResult.CARD_NOT_FOUND:
			//				case ERequestResult.CARD_NOT_FOUND_IN_COLLECTION:
			//					statusCode = -1;
			//					results.AddResult(404, result.ToString(), cardUID);
			//					nbErrors++;
			//					break;
			//				case ERequestResult.CARD_REMOVED:
			//					results.AddResult(200, result.ToString(), cardUID);
			//					break;
			//				default:
			//					results.AddResult(0, result.ToString(), cardUID);
			//					break;
			//			}
			//		}
			//	}
			//}

			//results.AddStatusCode(statusCode);
			//results.AddMessage($"Cards removed from collection with {nbErrors} errro(s).");

			//return results;
		}

		#endregion
	}
}
