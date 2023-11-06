/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using HotChocolate.Authorization;
using MagicAppAPI.Context;
using MagicAppAPI.Enums;
using MagicAppAPI.GraphQL.Queries.ReturnTypes;
using MagicAppAPI.Models;
using MagicAppAPI.Tools;

namespace MagicAppAPI.GraphQL.Queries
{
	/// <summary>Class that manages the retrieval of collection data.</summary>
	[ExtendObjectType("Query")]
	public class CollectionQuery
	{
		#region Public Methods

		/// <summary>Gets all collections for current user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <returns>A <see cref="CollectionQueryReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionQueryReturnType> GetMyCollections([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new CollectionQueryReturnType(404, "FAILURE: Current user not found.", 0, new List<Collection>());

			if (result.user == null)
				return new CollectionQueryReturnType(404, "FAILURE: An error occurred retrieving the user.", 0, new List<Collection>());

			var collections = context.Collections.Where(collection => collection.UserId == result.user.Id).ToList();

			return new CollectionQueryReturnType(200, $"SUCCESS: {collections.Count} collection(s).", collections.Count, collections);
		}

		/// <summary>Gets all collections for specific user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <returns>A <see cref="CollectionQueryReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionQueryReturnType> GetCollectionsByUserId([Service] MagicAppContext context, int userId)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionQueryReturnType(404, "FAILURE: User not found.", 0, new List<Collection>());

			var collections = context.Collections.Where(collection => collection.UserId == userId).ToList();

			return new CollectionQueryReturnType(200, $"SUCCESS: {collections.Count} collection(s).", collections.Count, collections);
		}

		/// <summary>Gets a collection for current user by knowing the collection identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="collectionId">Collection identifier.</param>
		/// <returns>A <see cref="CollectionQueryReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionQueryReturnType> GetCollectionForCurrentUserById([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new CollectionQueryReturnType(404, "FAILURE: Current user not found.", 0, new List<Collection>());

			if (result.user == null)
				return new CollectionQueryReturnType(404, "FAILURE: An error occurred retrieving the user.", 0, new List<Collection>());

			var collections = context.Collections.Where(collection => collection.UserId == result.user.Id && collection.Id == collectionId).ToList();

			return new CollectionQueryReturnType(200, $"SUCCESS: {collections.Count} collection(s).", collections.Count, collections);
		}

		/// <summary>Gets a collection for specific user by knowing the user identifier and the collection identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="collectionId">Collection identifier.</param>
		/// <returns>A <see cref="CollectionQueryReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionQueryReturnType> GetCollectionForUserById([Service] MagicAppContext context, int userId, int collectionId)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new CollectionQueryReturnType(404, "FAILURE: User not found.", 0, new List<Collection>());

			var collections = context.Collections.Where(collection => collection.UserId == user.Id && collection.Id == collectionId).ToList();

			return new CollectionQueryReturnType(200, $"SUCCESS: {collections.Count} collection(s).", collections.Count, collections);
		}

		#endregion Public Methods
	}
}
