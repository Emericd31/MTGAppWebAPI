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
	/// <summary>User object.</summary>
	public class User
	{
		#region Public Properties

		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>User login.</summary>
		public string Login { get; set; } = string.Empty;

		/// <summary>Firstname.</summary>
		public string FirstName { get; set; } = string.Empty;

		/// <summary>Lastname.</summary>
		public string LastName { get; set; } = string.Empty;

		/// <summary>Email address.</summary>
		public string Email { get; set; } = string.Empty;

		/// <summary>Password.</summary>
		public string Password { get; set; } = string.Empty;

		/// <summary>Date of registration.</summary>
		public string RegisterDate { get; set; } = string.Empty;

		/// <summary>Boolean indicating if the accound is registered.</summary>
		public bool IsRegistered { get; set; }

		/// <summary>List of collections.</summary>
		public IList<Collection> Collections { get; set; } = new List<Collection>();

		/// <summary>Property used for the intermediate table.</summary>
		public IList<UserRights> UserRights { get; set; } = new List<UserRights>();

		#endregion Public Properties
	}
}
