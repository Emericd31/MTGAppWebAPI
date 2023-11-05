/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.ComponentModel.DataAnnotations;

namespace MagicAppAPI.Models
{
	public class User
	{
		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>User login.</summary>
		public string Login { get; set; }

		/// <summary>Firstname.</summary>
		public string FirstName { get; set; }

		/// <summary>Lastname.</summary>
		public string LastName { get; set; }

		/// <summary>Email address.</summary>
		public string Email { get; set; }

		/// <summary>Password.</summary>
		public string Password { get; set; }

		/// <summary>Date of registration.</summary>
		public string RegisterDate { get; set; }

		/// <summary>Boolean indicating if the accound is registered.</summary>
		public bool IsRegistered { get; set; }

		/// <summary>List of collections.</summary>
		public IList<Collection> Collections { get; set; }

		/// <summary>Property used for the intermediate table.</summary>
		public IList<UserRights> UserRights { get; set; }
	}
}
