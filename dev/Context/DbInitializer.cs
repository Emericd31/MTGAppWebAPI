/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Models;
using MagicAppAPI.Tools;

namespace MagicAppAPI.Context
{
	public static class DbInitializer
	{
		public static void Initialize(MagicAppContext context)
		{
			#region Rights

			// Rights initialization
			if (!context.Rights.Any())
			{
				var rights = new Right[]
				{
					new Right { Name = "access_app", Domain = "application" },
					new Right { Name = "manage_members", Domain = "users" }
				};

				context.Rights.RemoveRange(context.Rights);
				context.Rights.AddRange(rights);
				context.SaveChanges();
			}

			#endregion Rights

			#region Users && Associated rights

			// Users and their associated rights initialization
			if (!context.Users.Any())
			{
				var access_app_right = context.Rights.FirstOrDefault(right => right.Name == "access_app");
				var access_app_right_id = (access_app_right != null) ? access_app_right.Id : -1;

				var manage_members_right = context.Rights.FirstOrDefault(r => r.Name == "manage_members");
				var manage_members_right_id = (manage_members_right != null) ? manage_members_right.Id : -1;

				var users = new User[]
				{
					new User
					{
						Login = "Admin",
						FirstName = "Admin",
						LastName = "Admin",
						Email = "admin@admin.com",
						Password = PasswordHashing.CreateHash("Admin"),
						RegisterDate = DateTime.Now.ToLocalTime().ToString(),
						IsRegistered = true
					},
					new User
					{
						Login = "User",
						FirstName = "User",
						LastName = "User",
						Email = "user@user.com",
						Password = PasswordHashing.CreateHash("User"),
						RegisterDate = DateTime.Now.ToLocalTime().ToString(),
						IsRegistered = true
					}
				};

				context.Users.RemoveRange(context.Users);
				context.Users.AddRange(users);
				context.SaveChanges();

				var user_admin = context.Users.FirstOrDefault(user => user.Login == "Admin" && user.Email == "admin@admin.com");
				var user_admin_id = (user_admin != null) ? user_admin.Id : -1;

				var user_user = context.Users.FirstOrDefault(user => user.Login == "User" && user.Email == "user@user.com");
				var user_user_id = (user_user != null) ? user_user.Id : -1;

				if (user_admin != null && user_user != null
					&& access_app_right != null && manage_members_right != null)
				{
					// User rights initialization
					var userRights = new UserRights[]
					{
						new UserRights { UserId = user_admin_id, User = user_admin, RightId = access_app_right_id, Right = access_app_right },
						new UserRights { UserId = user_admin_id, User = user_admin, RightId = manage_members_right_id, Right = manage_members_right },
						new UserRights { UserId = user_user_id, User = user_user, RightId = access_app_right_id, Right = access_app_right },
					};
					context.UserRights.RemoveRange(context.UserRights);
					context.UserRights.AddRange(userRights);
					context.SaveChanges();
				}

			}

			#endregion Users && Associated rights

			#region Card Types

			// Card types initialization
			if (!context.Types.Any())
			{
				var types = new Models.Type[]
				{
					new Models.Type { Value = "UNKNOWN" },
					new Models.Type { Value = "CREATURE" },
					new Models.Type { Value = "SORCERY" },
					new Models.Type { Value = "ENCHANTMENT" },
					new Models.Type { Value = "INSTANT" },
					new Models.Type { Value = "ARTIFACT" },
					new Models.Type { Value = "SIEGE" },
					new Models.Type { Value = "PLANESWALKER" },
					new Models.Type { Value = "LAND" },
					new Models.Type { Value = "LEGENDARY" },
				};

				context.Types.RemoveRange(context.Types);
				context.Types.AddRange(types);
				context.SaveChanges();
			}

			#endregion Card Types

			#region Card Colors

			// Card colors initialization
			if (!context.Colors.Any())
			{
				var colors = new Color[]
				{
					new Color { Value = "GREEN" },
					new Color { Value = "BLUE" },
					new Color { Value = "RED" },
					new Color { Value = "WHITE" },
					new Color { Value = "BLACK" },
					new Color { Value = "ARTIFACT" },
					new Color { Value = "BICOLOR" },
				};

				context.Colors.RemoveRange(context.Colors);
				context.Colors.AddRange(colors);
				context.SaveChanges();
			}

			#endregion Card Colors
		}
	}
}
