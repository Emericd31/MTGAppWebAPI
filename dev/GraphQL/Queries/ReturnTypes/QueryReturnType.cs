/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.Queries.ReturnTypes
{
	public class QueryReturnType
	{
		public int StatusCode { get; set; }

		public object? Data { get; set; }

		public QueryReturnType(int statusCode, object? data)
		{
			StatusCode = statusCode;
			Data = data;
		}
	}
}
