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
	[ExtendObjectType("Query")]
	public class UserQuery
	{
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		[Authorize(Roles = new[] { "manage_members" })]
		public async Task<IQueryable<User>> GetUsers([Service] MagicAppContext context)
		{
			return context.Users;
		}
	}
}
