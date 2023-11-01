/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	public class TokenSettings
	{
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public string Key { get; set; }
	}
}
