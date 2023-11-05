/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.Queries.ReturnTypes
{
	/// <summary>Result of general queries.</summary>
	public class QueryReturnType
	{
		#region Public Properties

		/// <summary>Status code.</summary>
		public int StatusCode { get; set; }

		/// <summary>Data.</summary>
		public object? Data { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="data">Data.</param>
		public QueryReturnType(int statusCode, object? data)
		{
			StatusCode = statusCode;
			Data = data;
		}

		#endregion Constructor
	}
}
