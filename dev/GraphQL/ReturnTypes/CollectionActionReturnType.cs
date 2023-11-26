/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.ReturnTypes
{
	/// <summary>Collection result for query.</summary>
	public class CollectionActionReturnType : BaseReturnType
	{
		#region Class Data

		/// <summary>Object containing the query data.</summary>
		/// <remarks>Result of an action performed on a card (ADD, REMOVE).</remarks>
		public class CollectionActionData
		{
			#region Public Properties

			/// <summary>Status code.</summary>
			public int StatusCode { get; set; }

			/// <summary>Message.</summary>
			public string Message { get; set; }

			/// <summary>Card unique identifier.</summary>
			public string CardUID { get; set; }

			#endregion Public Properties

			#region Constructor

			/// <summary>Constructor.</summary>
			/// <param name="statusCode">Status code.</param>
			/// <param name="message">Message.</param>
			/// <param name="cardUID">Card unique identifier.</param>
			public CollectionActionData(int statusCode, string message, string cardUID)
			{
				StatusCode = statusCode;
				Message = message;
				CardUID = cardUID;
			}

			#endregion Constructor
		}

		#endregion Class Data

		#region Public Properties

		/// <summary>List of <see cref="CollectionActionData"/> results.</summary>
		public List<CollectionActionData> Data { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public CollectionActionReturnType() : base()
		{
			Data = new List<CollectionActionData>();
		}

		/// <summary>Constructor.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Message.</param>
		public CollectionActionReturnType(int statusCode, string message) : base(statusCode, message)
		{
			Data = new List<CollectionActionData>();
		}

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

		/// <summary>Adds a result in the list of results.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Message.</param>
		/// <param name="cardUID">Card unique identifier.</param>
		public void AddResult(int statusCode, string message, string cardUID)
		{
			Data.Add(new CollectionActionData(statusCode, message, cardUID));
		}

		/// <summary>Changes the request status to success.</summary>
		public void SetToSuccess()
		{
			StatusCode = 200;
			Message = "SUCCESS";
		}

		#endregion Public Methods
	}
}
