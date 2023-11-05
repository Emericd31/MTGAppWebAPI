/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.Mutations.ReturnTypes
{
	/// <summary>Result of an action performed on a mutation.</summary>
	public class MutationReturnTypeWithId : MutationReturnType
    {
		#region Public Properties

		/// <summary>Identifier.</summary>
		public int Id { get; set; }

		#endregion Public Properties

		#region Constructor.

		/// <summary>Constructor.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Meessage.</param>
		/// <param name="id">Identifier.</param>
		public MutationReturnTypeWithId(int statusCode, string message, int id) : base(statusCode, message)
        {
            Id = id;
        }

		#endregion Constructor
	}
}
