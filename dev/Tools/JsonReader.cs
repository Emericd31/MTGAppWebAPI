/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.Text.Json;

namespace MagicAppAPI.Tools
{
	public static class JsonReader
	{
		public static string TryGetStringFromJsonElement(JsonElement? jsonElement)
		{
			try
			{
				if (jsonElement == null)
					return "";

				return jsonElement.ToString();
			}
			catch
			{
				return "";
			}
		}

		public static bool TryGetBoolFromJsonElement(JsonElement? jsonElement)
		{
			try
			{
				if (jsonElement == null)
					return false;

				if (bool.TryParse(jsonElement.ToString(), out bool isDigital))
					return isDigital;
				return false;
			}
			catch
			{
				return false;
			}
		}

		public static long TryGetLongFromJsonElement(JsonElement? jsonElement)
		{
			try
			{
				if (jsonElement == null)
					return 0;

				if (long.TryParse(jsonElement.ToString(), out long result))
					return result;
				return 0;
			}
			catch
			{
				return 0;
			}
		}
	}
}
