/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL
{
	/// <summary>Class that handles Graphql errors.</summary>
	public class GraphQLErrorFilter : IErrorFilter
	{
		#region Public Methods

		/// <summary>Methods called to retrieve errors.</summary>
		/// <param name="error">Error.</param>
		/// <returns>An object implemeting <see cref="IError"/>.</returns>
		public IError OnError(IError error)
		{
			if (error?.Exception?.Message != null)
				return error.WithMessage(error.Exception.Message);

			if (error?.Message != null)
				return error.WithMessage(error.Message);

			return ErrorBuilder.New().SetMessage("An error occurred").Build();
		}

		#endregion Public Methods
	}
}
