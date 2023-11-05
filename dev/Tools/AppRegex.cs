/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Tools
{
	/// <summary>Class that handles all the regex used in the application.</summary>
	public static class AppRegex
	{
		#region Public Properties

		/// <summary>Dictionary of regex.</summary>
		public static Dictionary<string, string> Regex = new Dictionary<string, string>
		{
		  {"email", "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"}
		};

		#endregion Public Properties
	}
}
