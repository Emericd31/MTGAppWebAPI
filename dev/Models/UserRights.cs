/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>User right object.</summary>
	public class UserRights
	{
		#region Public Properties

		/// <summary>User's identifier.</summary>
		public int UserId { get; set; }

		/// <summary>User object.</summary>
		public User? User { get; set; }

		/// <summary>Right's identifier.</summary>
		public int RightId { get; set; }

		/// <summary>Right object.</summary>
		public Right? Right { get; set; }

		#endregion Public Properties
	}
}
