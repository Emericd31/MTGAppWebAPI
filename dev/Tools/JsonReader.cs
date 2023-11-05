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
	/// <summary>Class that handles methods to read JSON objects.</summary>
	public static class JsonReader
	{
		#region Public Methods

		/// <summary>Tries to get a string from a JSON element.</summary>
		/// <param name="jsonElement">JSON element.</param>
		/// <returns>The string result.</returns>
		/// <remarks>Return empty string if error.</remarks>
		public static string TryGetStringFromJsonElement(JsonElement? jsonElement)
		{
			try
			{
				return jsonElement?.ToString() ?? string.Empty;
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>Tries to get a boolean from a JSON element.</summary>
		/// <param name="jsonElement">JSON element.</param>
		/// <returns>The boolean result.</returns>
		/// <remarks>Return false if error.</remarks>
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

		/// <summary>Tries to get a long from a JSON element.</summary>
		/// <param name="jsonElement">JSON element.</param>
		/// <returns>The long result.</returns>
		/// <remarks>Return 0 if error.</remarks>
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

		#endregion Public Methods
	}
}
