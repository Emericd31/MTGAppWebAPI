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
using Microsoft.EntityFrameworkCore;
using Orchestre.API.Tools;

namespace MagicAppAPI.GraphQL.Mutations
{
    /// <summary>Class that handles users, including all additions, modifications and deletions.</summary>
    [ExtendObjectType("Mutation")]
	public class UserMutation
	{
		#region Private Properties

		/// <summary>Application settings.</summary>
		private readonly IConfiguration _configuration;

		#endregion

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="configuration">Application settings.</param>
		public UserMutation(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		#endregion Constructor

		#region Public Methods

		#region User Registration

		/// <summary>Adds a user in database.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="login">User login.</param>
		/// <param name="firstName">User firstname.</param>
		/// <param name="lastName">User lastname.</param>
		/// <param name="email">User email.</param>
		/// <param name="password">User password.</param>
		/// <returns>A <see cref="BaseReturnTypeWithId"/> object containing result of the request.</returns>
		public async Task<BaseReturnTypeWithId> AddUser([Service] MagicAppContext context, string login, string firstName, string lastName, string email, string password)
		{
			// Checks email syntax validity
			if (!LoginTools.CheckEmailValidity(email))
				return new BaseReturnTypeWithId(403, String.Format("FAILURE: email {0} is not valid.", email), -1);

			// Seeks email in database
			if (context.Users.Any(usr => usr.Email.ToLower() == email))
				return new BaseReturnTypeWithId(403, string.Format("FAILURE: email {0} is already used.", email), -1);

			// Seeks login in database
			if (context.Users.Any(usr => usr.Login.ToLower() == login))
				return new BaseReturnTypeWithId(403, string.Format("FAILURE: login {0} is already used.", login), -1);

			// Finds user base right
			var accessApp = await context.Rights.FirstOrDefaultAsync(right => right.Name.ToLower() == "access_app").ConfigureAwait(false);
			if (accessApp is null)
				return new BaseReturnTypeWithId(404, "FAILURE: \"accessApp\" right does not exist.", -1);

			// Creates new user
			User newUser = new User
			{
				Login = login,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				Password = PasswordHashing.CreateHash(password),
				RegisterDate = DateTime.Now.ToLocalTime().ToString(),
				IsRegistered = false
			};
			context.Users.Add(newUser);

			// Associates base rights to user
			UserRights userRight = new UserRights
			{
				UserId = newUser.Id,
				User = newUser,
				RightId = accessApp.Id,
				Right = accessApp
			};
			context.UserRights.Add(userRight);

			// Creates user collection
			Collection userCollection = new Collection
			{
				Name = "MyCollection",
				Description = "All cards owned by the player",
				NbCards = 0,
				EURPrice = 0.0f,
				EURCardNotValued = 0,
				USDPrice = 0.0f,
				USDCardNotValued = 0,
				UserId = newUser.Id,
				User = newUser
			};
			context.Collections.Add(userCollection);

			await context.SaveChangesAsync();

			EmailSender.SendEmailVerificationMessage(email, SettingsHelper.GetEmailSignature(_configuration));

			return new BaseReturnTypeWithId(200, "SUCCESS: User created.", newUser.Id);
		}

		/// <summary>Verifies an account by knowing his token.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="token">User token.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<BaseReturnType> VerifyUser([Service] MagicAppContext context, string token)
		{
			var user = TokenGenerator.GetUserFromTokenWithEmailClaim(token, context);

			if (user == null)
			{
				return new BaseReturnType(404, "User not found.");
			}
			else
			{
				if (user.IsRegistered)
				{
					return new BaseReturnType(405, "User already validated.");
				}
				else
				{
					user.IsRegistered = true;
					await context.SaveChangesAsync();
					return new BaseReturnType(200, "Ok");
				}
			}
		}

		#endregion User Registration

		#region User Modification

		/// <summary>Modifies the current user's email.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Context accessor.</param>
		/// <param name="newEmail">New email address.</param>
		/// <param name="password">Current password.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<BaseReturnType> ModifyCurrentUserEmail([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, string newEmail, string password)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new BaseReturnType(404, string.Format("FAILURE: Current user not found.", -1));

			if (!PasswordHashing.ValidatePassword(password, result.user.Password))
			{
				return new BaseReturnType(403, "FAILURE: Wrong password.");
			}
			else if (result.user.Email == newEmail)
			{
				return new BaseReturnType(403, "FAILURE: Unable to change email address because it's the same as the previous one.");
			}

			if (context.Users.Any(user => user.Email.ToLower() == newEmail.ToLower()))
				return new BaseReturnType(400, String.Format("FAILURE: The email address: {0} is already associated with an account.", newEmail));

			result.user.IsRegistered = false;
			result.user.Email = newEmail;
			await context.SaveChangesAsync();

			EmailSender.SendEmailVerificationMessage(result.user.Email, SettingsHelper.GetEmailSignature(_configuration));

			return new BaseReturnType(200, "SUCCESS: User modified.");
		}

		/// <summary>Modifies the user's email by knwoing his identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Context accessor.</param>
		/// <param name="newEmail">New email address.</param>
		/// <param name="password">Current password.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_members" })]
		public async Task<BaseReturnType> ModifyUserEmailById([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int userId, string newEmail, string password)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new BaseReturnType(404, string.Format("FAILURE: Current user not found.", -1));

			var userToEdit = await context.Users.FindAsync(userId);
			if (userToEdit is null)
				return new BaseReturnType(404, "FAILURE: User not found.");

			if (!PasswordHashing.ValidatePassword(password, result.user.Password))
				return new BaseReturnType(403, "FAILURE: Wrong password.");

			if (userToEdit.Email == newEmail)
				return new BaseReturnType(403, "FAILURE: Unable to change email address because it is the same as the previous one.");

			if (context.Users.Any(user => user.Email.ToLower() == newEmail.ToLower()))
				return new BaseReturnType(400, String.Format("FAILURE: The email address: {0} is already associated with an account.", newEmail));

			userToEdit.IsRegistered = false;
			userToEdit.Email = newEmail;
			await context.SaveChangesAsync();

			EmailSender.SendEmailVerificationMessage(userToEdit.Email, SettingsHelper.GetEmailSignature(_configuration));

			return new BaseReturnType(200, "SUCCESS: User modified.");
		}

		/// <summary>Modifies the current user's password.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Context accessor.</param>
		/// <param name="currentPassword">Current user password.</param>
		/// <param name="newPassword">New user password.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<BaseReturnType> ModifyCurrentUserPassword([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, string currentPassword, string newPassword)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new BaseReturnType(404, string.Format("FAILURE: Current user not found.", -1));

			if (!PasswordHashing.ValidatePassword(currentPassword, result.user.Password))
				return new BaseReturnType(403, "FAILURE: The current password is wrong.");

			if (PasswordHashing.ValidatePassword(newPassword, result.user.Password))
				return new BaseReturnType(403, "FAILURE: Unable to change password because it's the same as previous password.");

			result.user.Password = PasswordHashing.CreateHash(newPassword);
			await context.SaveChangesAsync();

			return new BaseReturnType(200, "SUCCESS: Password changed.");
		}

		/// <summary>Modifies the password of a user by knowing his identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Context accessor.</param>
		/// <param name="userId">User identifier.</param>
		/// <param name="password">Password of the user asking for modification.</param>
		/// <param name="newPassword">New password of the user concerned by the modification.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_members" })]
		public async Task<BaseReturnType> ModifyPasswordByUserId([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, int userId, string password, string newPassword)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new BaseReturnType(404, string.Format("FAILURE: Current user not found.", -1));

			var userToEdit = await context.Users.FindAsync(userId);
			if (userToEdit is null)
				return new BaseReturnType(404, "FAILURE: User not found.");

			if (!PasswordHashing.ValidatePassword(password, result.user.Password))
				return new BaseReturnType(403, "FAILURE: Wrong password.");

			userToEdit.Password = PasswordHashing.CreateHash(newPassword);
			await context.SaveChangesAsync();

			return new BaseReturnType(200, "SUCCESS: Password changed.");
		}

		/// <summary>Modifies the current user's information.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Context accessor.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<BaseReturnType> ModifyCurrentUserInformation([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor, string firstname, string lastname)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new BaseReturnType(404, string.Format("FAILURE: User not found.", -1));

			result.user.FirstName = firstname;
			result.user.LastName = lastname;
			await context.SaveChangesAsync();

			return new BaseReturnType(200, "SUCCESS: User modified.");
		}

		/// <summary>Modifies the information of a user by knowing his identifier.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">Identifier of the user to edit.</param>
		/// <param name="firstname">New firstname of the user.</param>
		/// <param name="lastname">New lastname of the user.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_members" })]
		public async Task<BaseReturnType> ModifyUserInformationById([Service] MagicAppContext context, int userId, string firstname, string lastname)
		{
			var userToEdit = await context.Users.FindAsync(userId);

			if (userToEdit is null)
				return new BaseReturnType(404, "FAILURE: User not found.");

			userToEdit.FirstName = firstname;
			userToEdit.LastName = lastname;
			await context.SaveChangesAsync();

			return new BaseReturnType(200, "SUCCESS: User modified.");
		}

		#endregion User Modification

		#region User Delete

		/// <summary>Deletes the current user.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="httpContextAccessor">Context accessor.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize]
		public async Task<BaseReturnType> DeleteCurrentUser([Service] MagicAppContext context, [Service] IHttpContextAccessor httpContextAccessor)
		{
			var result = await HttpAccessorTools.GetUserByAccessor(context, httpContextAccessor).ConfigureAwait(false);
			if (result.result != EHttpAccessorResult.SUCCESS || result.user == null)
				return new BaseReturnType(404, string.Format("FAILURE: User not found.", -1));

			context.Users.Remove(result.user);
			await context.SaveChangesAsync();

			return new BaseReturnType(200, "SUCCESS: User removed.");
		}

		/// <summary>Deletes a user by knowing his identifer.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="userId">User identifier.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		[Authorize(Roles = new[] { "manage_members" })]
		public async Task<BaseReturnType> DeleteUserById([Service] MagicAppContext context, int userId)
		{
			var user = await context.Users.FindAsync(userId);
			if (user is null)
				return new BaseReturnType(404, String.Format("FAILURE: Unable to delete user because id:{0} does not match with any user in the dataset.", userId));

			context.Users.Remove(user);
			await context.SaveChangesAsync();

			return new BaseReturnType(200, "SUCCESS: User removed.");
		}

		#endregion User Delete

		#region Password

		/// <summary>Resets a user's password knowing their email address.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="email">Email to reset password.</param>
		/// <returns>A <see cref="BaseReturnType"/> object containing result of the request.</returns>
		public async Task<BaseReturnType> ResetPassword([Service] MagicAppContext context, string email)
		{
			var user = context.Users.FirstOrDefault(u => u.Email.Equals(email));

			if (user == null)
				return new BaseReturnType(404, "FAILURE: User not found.");

			var newPassword = LoginTools.GeneratePassword(15);
			user.Password = PasswordHashing.CreateHash(newPassword);
			await context.SaveChangesAsync();

			EmailSender.SendResetPasswordMessage(email, newPassword, SettingsHelper.GetEmailSignature(_configuration));

			return new BaseReturnType(200, "");
		}

		#endregion Password

		#endregion Public Methods
	}
}
