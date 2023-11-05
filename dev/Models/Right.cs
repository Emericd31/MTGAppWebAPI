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
	/// <summary>Right object.</summary>
	public class Right
	{
		#region Public Properties

		[Key]
		/// <summary>Database's identifier.</summary>
		public int Id { get; set; }

		/// <summary>Name of the right.</summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>Domain of the right.</summary>
		public string Domain { get; set; } = string.Empty;

		/// <summary>Property used for the intermediate table.</summary>
		public IList<UserRights> UserRights { get; set; } = new List<UserRights>();

		#endregion Public Properties
	}
}
