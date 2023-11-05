/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Context;
using MagicAppAPI.GraphQL.Mutations.ReturnTypes;
using MagicAppAPI.Models;
using MagicAppAPI.Tools;
using Orchestre.API.Tools;
using System.Security.Claims;

namespace MagicAppAPI.GraphQL.Mutations
{
	/// <summary>Class that handles login.</summary>
	[ExtendObjectType("Mutation")]
	public class LoginMutation
	{
		#region Public Methods

		/// <summary>Connects a user knowing their email and password.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="email">Email address.</param>
		/// <param name="password">Password.</param>
		/// <returns>A <see cref="LoginMutationReturnType"/> object containing result of the request.</returns>
		public LoginMutationReturnType Login([Service] MagicAppContext context, string email, string password)
		{
			var currentUser = context.Users.FirstOrDefault(user => user.Email.ToLower().Equals(email.ToLower()));

			if (currentUser != null && PasswordHashing.ValidatePassword(password, currentUser.Password))
			{
				List<Right> userRights = LoginTools.GetRightsByUserId(currentUser, context);

				if (userRights == null || userRights.Count == 0 || !userRights.Any(right => right.Name == "access_app"))
					return new LoginMutationReturnType(404, "FAILURE: The user's has no associated rights.", -1, "");

				List<Claim> claims = LoginTools.GetClaimsFromUserRights(userRights);
				claims.Add(new Claim(ClaimTypes.Email, email));
				claims.Add(new Claim("userId", currentUser.Id.ToString()));
				String token = TokenGenerator.GenerateJwtToken(claims);

				if (currentUser.IsRegistered)
				{
					return new LoginMutationReturnType(200, "SUCCESS", currentUser.Id, token);
				}
				else
				{
					return new LoginMutationReturnType(403, "FAILURE: Account not confirmed.", currentUser.Id, "");
				}
			}

			return new LoginMutationReturnType(400, "FAILURE: Invalid credentials.", -1, "");
		}

		#endregion Public Methods
	}
}
