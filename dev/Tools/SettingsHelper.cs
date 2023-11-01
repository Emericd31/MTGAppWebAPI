/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using Microsoft.Extensions.Configuration;

namespace MagicAppAPI.Tools
{
	public static class SettingsHelper
	{
		private const string EMAIL_SETTINGS_SECTION = "EmailSettings";

		private const string EMAIL_SIGNATURE_KEY = "Signature";

		public static string GetEmailSignature(IConfiguration configuration)
		{
			var emailSettings = configuration.GetSection("EmailSettings");
			return emailSettings.GetValue<string>("Signature");
		}
	}
}
