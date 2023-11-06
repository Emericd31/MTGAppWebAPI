/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.Security.Claims;
using MagicAppAPI.Context;
using MagicAppAPI.Enums;
using MagicAppAPI.Models;

namespace MagicAppAPI.Tools
{
	/// <summary>Class that handles tools for IHttpContextAccessor object.</summary>
	public static class HttpAccessorTools
	{
		#region Public Methods

		/// <summary>Gets a user given a HttpContextAccessor.</summary>
		/// <param name="context">Database context.</param>
		/// <param name="accessor">HttpContextAccessor.</param>
		/// <returns>A tuple containing the <see cref="EHttpAccessorResult"/> and the <see cref="User"/> (null if not found).</returns>
		public static async Task<(EHttpAccessorResult result, User? user)> GetUserByAccessor(MagicAppContext context, IHttpContextAccessor accessor)
		{
			if (accessor == null)
				return (EHttpAccessorResult.NULL_HTTP_ACCESSOR, null);

			int currentUserId = -1;
			if (int.TryParse(accessor?.HttpContext?.User.FindFirstValue("userId"), out int receivedId))
				currentUserId = receivedId;

			var currentUser = await context.Users.FindAsync(currentUserId);
			if (currentUser is null)
				return (EHttpAccessorResult.USER_NOT_FOUND, null);

			return (EHttpAccessorResult.SUCCESS, currentUser);
		}

		#endregion Public Methods
	}
}
