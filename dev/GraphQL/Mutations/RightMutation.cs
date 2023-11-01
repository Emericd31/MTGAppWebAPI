/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using HotChocolate.Authorization;
using MagicAppAPI.Context;
using MagicAppAPI.GraphQL.Mutations.ReturnTypes;
using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL.Mutations
{
	/// <summary>Class that handles users' rights.</summary>
	[ExtendObjectType("Mutation")]
	public class RightMutation
	{
		#region Public Methods

		/// <summary>Assigns a right to a user by knowing his identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="idUser">User identifer.</param>
		/// <param name="idRight">Right identifier.</param>
		/// <returns></returns>
		[Authorize(Roles = new[] { "manage_members" })]
		public async Task<MutationReturnType> AssignRightToUser([Service] MagicAppContext context, int idUser, int idRight)
		{
			var right = await context.Rights.FindAsync(idRight);
			if (right is null)
				return new MutationReturnType(404, String.Format("FAILURE: Unable to assign right to user because id:{0} does not match with any right in the dataset.", idRight));

			var user = await context.Users.FindAsync(idUser);
			if (user is null)
				return new MutationReturnType(404, String.Format("FAILURE: Unable to assign right to user because id:{0} does not match with any user in the dataset.", idUser));

			if (context.UserRights.Any(ur => ur.UserId == user.Id && ur.RightId == right.Id))
				return new MutationReturnType(403, "FAILURE: Unable to assign right to user because user already have this right.");

			UserRights newUserRight = new UserRights
			{
				RightId = idRight,
				Right = right,
				UserId = idUser,
				User = user
			};
			context.UserRights.Add(newUserRight);
			await context.SaveChangesAsync();

			return new MutationReturnType(200, "SUCCESS: right assigned to user.");
		}

		/// <summary>Removes a right from a user by knowing his identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="idUser">User identifer.</param>
		/// <param name="idRight">Right identifier.</param>
		[Authorize(Roles = new[] { "manage_members" })]
		public async Task<MutationReturnType> RemoveRightFromUser([Service] MagicAppContext context, int idUser, int idRight)
		{
			var right = await context.Rights.FindAsync(idRight);
			if (right is null)
				return new MutationReturnType(404, String.Format("FAILURE: Unable to remove right to role because id:{0} does not match with any right in the dataset.", idRight));

			var user = await context.Users.FindAsync(idUser);
			if (user is null)
				return new MutationReturnType(404, String.Format("FAILURE: Unable to remove right from user because id:{0} does not match with any user in the dataset.", idUser));

			var userRight = context.UserRights.FirstOrDefault(input => input.UserId == idUser && input.RightId == idRight);
			if (userRight is null)
				return new MutationReturnType(404, "FAILURE: Unable to remove right from user because the user does not have this right");

			context.UserRights.Remove(userRight);
			await context.SaveChangesAsync();

			return new MutationReturnType(200, "SUCCESS: right removed from user.");
		}

		#endregion Public Methods
	}
}
