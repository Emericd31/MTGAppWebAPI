/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Set object.</summary>
	public class Set
	{
		#region Public Properties

		/// <summary>Name.</summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>Code.</summary>
		public string Code { get; set; } = string.Empty;

		/// <summary>Type.</summary>
		public string Type { get; set; } = string.Empty;

		/// <summary>Image URL.</summary>
		public string ImgURL { get; set; } = string.Empty;

		/// <summary>Release date.</summary>
		public string ReleaseDate { get; set; } = string.Empty;

		/// <summary>Card count.</summary>
		public long CardCount { get; set; }

		/// <summary>Parent set code.</summary>
		public string ParentSetCode { get; set; } = string.Empty;

		/// <summary>Boolean indicating if the set is digital.</summary>
		public bool IsDigital { get; set; }

		#endregion Public Propertiess
	}
}
