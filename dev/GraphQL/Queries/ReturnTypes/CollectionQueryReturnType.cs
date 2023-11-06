/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL.Queries.ReturnTypes
{
	/// <summary>Result of collection queries.</summary>
	public class CollectionQueryReturnType
	{
		#region Public Properties

		/// <summary>Status code.</summary>
		public int StatusCode { get; set; }

		/// <summary>Message.</summary>
		public string Message { get; set; } = string.Empty;

		/// <summary>Number of collections in result.</summary>
		public int NbCollections { get; set; }

		/// <summary>Collections in result.</summary>
		public List<Collection> Collections { get; set; } = new List<Collection>();

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public CollectionQueryReturnType() { }

		/// <summary>Constructor.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Message.</param>
		/// <param name="nbCollections">Number of collections in result.</param>
		/// <param name="collections">Collections in result.</param>
		public CollectionQueryReturnType(int statusCode, string message, int nbCollections, List<Collection> collections)
		{
			StatusCode = statusCode;
			Message = message;
			NbCollections = nbCollections;
			Collections = collections;
		}

		#endregion Constructor
	}
}
