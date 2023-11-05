/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Tools
{
	/// <summary>Class that handles tools for application settings.</summary>
	public static class SettingsHelper
	{
		#region Constants 

		/// <summary>Email settings section name.</summary>
		private const string EMAIL_SETTINGS_SECTION = "EmailSettings";

		/// <summary>Email signature key name.</summary>
		private const string EMAIL_SIGNATURE_KEY = "Signature";

		#endregion Constants

		#region Public Methods

		/// <summary>Gets email signature from configuration.</summary>
		/// <param name="configuration">Configuration.</param>
		/// <returns>The email signature.</returns>
		public static string GetEmailSignature(IConfiguration configuration)
		{
			var emailSettings = configuration.GetSection(EMAIL_SETTINGS_SECTION);
			return emailSettings.GetValue<string>(EMAIL_SIGNATURE_KEY);
		}

		#endregion Public Methods
	}
}
