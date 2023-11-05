/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Token setting object.</summary>
	public class TokenSettings
	{
		#region Public Properties

		/// <summary>Issuer.</summary>
		public string? Issuer { get; set; }

		/// <summary>Audience.</summary>
		public string? Audience { get; set; }

		/// <summary>Key.</summary>
		public string? Key { get; set; }

		#endregion Public Properties
	}
}
