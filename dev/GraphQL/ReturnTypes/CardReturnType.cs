/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Models;

namespace MagicAppAPI.GraphQL.ReturnTypes
{
	/// <summary>Card result for query.</summary>
	public class CardReturnType : BaseReturnType
	{
		#region Class Data

		/// <summary>Object containing the query data.</summary>
		public class CardData
		{
			#region Public Properties

			/// <summary>Number of resulting cards.</summary>
			public long NumberOfCards { get; set; }

			/// <summary>List of resulting cards.</summary>
			public List<Card> Cards { get; set; } = new List<Card>();

			#endregion Public Properties

			#region Constructor

			/// <summary>Default constructor.</summary>
			public CardData() { }

			#endregion Constructor
		}

		#endregion Class Data

		#region Public Properties

		/// <summary>Query data.</summary>
		public CardData Data { get; set; } = new CardData();

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public CardReturnType() { }

		/// <summary>Constructor.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Message.</param>
		public CardReturnType(int statusCode, string message) : base(statusCode, message) { }

		#endregion Constructor

		#region Public Methods

		/// <summary>Changes the general status code.</summary>
		/// <param name="statusCode">Status code.</param>
		public void ChangeStatusCode(int statusCode)
		{
			StatusCode = statusCode;
		}

		/// <summary>Changes the general message.</summary>
		/// <param name="message">Message.</param>
		public void ChangeMessage(string message)
		{
			Message = message;
		}

		/// <summary>Changes the query status to success.</summary>
		public void SetToSuccess()
		{
			StatusCode = 200;
			Message = "SUCCESS";
		}

		#endregion Public Methods
	}
}
