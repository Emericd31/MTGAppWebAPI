/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Context;
using MagicAppAPI.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MagicAppAPI.Tools
{
	public static class LoginTools
	{
		public static string GeneratePassword(int nbChar)
		{
			var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var Charsarr = new char[nbChar];
			var random = new Random();

			for (int i = 0; i < Charsarr.Length; i++)
			{
				Charsarr[i] = characters[random.Next(characters.Length)];
			}
			return new string(Charsarr);
		}

		public static List<Right> GetRightsByUserId(User currentUser, [Service] MagicAppContext context)
		{
			try
			{
				List<UserRights> userRights = context.UserRights.Where(input => input.UserId == currentUser.Id).ToList();

				List<Right> rights = new List<Right>();
				foreach (UserRights rr in userRights)
				{
					var right = context.Rights.FirstOrDefault(input => input.Id == rr.RightId);
					rights.Add(right);
				}
				return rights;
			}
			catch
			{
				return new List<Right>();
			}
		}

		public static List<Claim> GetClaimsFromUserRights(List<Right> userRights)
		{
			List<Claim> claims = new List<Claim>();
			foreach (Right right in userRights)
			{
				claims.Add(new Claim(ClaimTypes.Role, right.Name));
			}
			return claims;
		}

		/// <summary>Checks if the given email has correct format.</summary>
		/// <param name="email">Email to check.</param>
		/// <returns>True if the given email is valid (false otherwise).</returns>
		public static bool CheckEmailValidity(string email)
		{
			string emailRegex = AppRegex.Regex["email"];
			return Regex.IsMatch(email, emailRegex);
		}
	}
}
