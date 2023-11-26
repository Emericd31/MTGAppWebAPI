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
using MagicAppAPI.GraphQL.ReturnTypes;
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
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionReturnType> GetMyCollections([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor)
		{
			var result = new CollectionReturnType();

			try
			{
				var extractUser = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
				if (extractUser.result != EHttpAccessorResult.SUCCESS || extractUser.user == null)
					return new CollectionReturnType(404, "FAILURE: Current user not found.");

				var collections = context.Collections.Where(collection => collection.UserId == extractUser.user.Id).ToList();

				result.Data.NbCollections = collections.Count;
				result.Data.Collections = collections;
				result.SetToSuccess();
			}
			catch
			{
				// Do nothing
			}

			return result;
		}

		/// <summary>Gets all collections for specific user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionReturnType> GetCollectionsByUserId([Service] MagicAppContext context, int userId)
		{
			var result = new CollectionReturnType();

			try
			{
				var user = await context.Users.FindAsync(userId);
				if (user is null)
					return new CollectionReturnType(404, "FAILURE: User not found.");

				var collections = context.Collections.Where(collection => collection.UserId == userId).ToList();

				result.Data.NbCollections = collections.Count;
				result.Data.Collections = collections;
				result.SetToSuccess();
			}
			catch
			{
				// Do nothing
			}

			return result;
		}

		/// <summary>Gets a collection for current user by knowing the collection identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Http context accessor.</param>
		/// <param name="collectionId">Collection identifier.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<CollectionReturnType> GetCollectionForCurrentUserById([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int collectionId)
		{
			var result = new CollectionReturnType();

			try
			{
				var extractUser = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
				if (extractUser.result != EHttpAccessorResult.SUCCESS || extractUser.user == null)
					return new CollectionReturnType(404, "FAILURE: Current user not found.");

				var collections = context.Collections.Where(collection => collection.UserId == extractUser.user.Id && collection.Id == collectionId).ToList();

				result.Data.NbCollections = collections.Count;
				result.Data.Collections = collections;
				result.SetToSuccess();
			}
			catch (Exception)
			{
				// Do nothing
			}

			return result;
		}

		/// <summary>Gets a collection for specific user by knowing the user identifier and the collection identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="collectionId">Collection identifier.</param>
		/// <returns>A <see cref="CollectionReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_collection" })]
		public async Task<CollectionReturnType> GetCollectionForUserById([Service] MagicAppContext context, int userId, int collectionId)
		{
			var result = new CollectionReturnType();

			try
			{
				var user = await context.Users.FindAsync(userId);
				if (user is null)
					return new CollectionReturnType(404, "FAILURE: User not found.");

				var collections = context.Collections.Where(collection => collection.UserId == user.Id && collection.Id == collectionId).ToList();

				result.Data.NbCollections = collections.Count;
				result.Data.Collections = collections;
				result.SetToSuccess();
			}
			catch
			{
				// Do nothing
			}

			return result;
		}

		#endregion Public Methods
	}
}
