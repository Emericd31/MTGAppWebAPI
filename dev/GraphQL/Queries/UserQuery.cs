/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using HotChocolate.Authorization;
using MagicAppAPI.Context;
using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL.Queries
{
	/// <summary>Class that manages the retrieval of user data.</summary>
	[ExtendObjectType("Query")]
	public class UserQuery
	{
		#region Public Methods

		/// <summary>Gets all users.</summary>
		/// <param name="context">Database context.</param>
		/// <returns>The list of <see cref="User"/> objects.</returns>
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[Authorize(Roles = new[] { "manage_members" })]
		public IQueryable<User> GetUsers([Service] MagicAppContext context)
		{
			return context.Users;
		}

		#endregion Public Methods
	}
}
