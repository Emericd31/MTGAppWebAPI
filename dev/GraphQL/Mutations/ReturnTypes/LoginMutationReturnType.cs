/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.Mutations.ReturnTypes
{
	/// <summary>Result of an action performed on an account (LOGIN).</summary>
	public class LoginMutationReturnType : MutationReturnTypeWithId
	{
		#region Public Properties

		/// <summary>Token.</summary>
		public string Token { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Message.</param>
		/// <param name="id">Identifier.</param>
		/// <param name="token">Token.</param>
		public LoginMutationReturnType(int statusCode, string message, int id, string token) : base(statusCode, message, id)
        {
            Token = token;
        }

		#endregion Constructor
	}
}
