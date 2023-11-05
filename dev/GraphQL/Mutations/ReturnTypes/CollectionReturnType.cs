/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.GraphQL.Mutations.ReturnTypes
{
	/// <summary>Result of an action performed on a card (ADD, REMOVE).</summary>
	public class CardActionResult
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
		public CardActionResult(int statusCode, string message, string cardUID)
		{
			StatusCode = statusCode;
			Message = message;
			CardUID = cardUID;
		}

		#endregion Constructor
	}

	/// <summary>Result of an action performed on a collection (ADD, REMOVE).</summary>
	public class CollectionReturnType
	{
		#region Public Properties

		/// <summary>Status code.</summary>
		public int StatusCode { get; set; }

		/// <summary>Message.</summary>
		public string Message { get; set; }

		/// <summary>List of <see cref="CardActionResult"/> results.</summary>
		public List<CardActionResult> Results { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		public CollectionReturnType()
		{
			Results = new List<CardActionResult>();
			Message = string.Empty;
		}

		/// <summary>Constructor.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Message.</param>
		public CollectionReturnType(int statusCode, string message)
		{
			StatusCode = statusCode;
			Message = message;
			Results = new List<CardActionResult>();
		}

		#endregion Constructor

		#region Public Methods

		/// <summary>Adds the general status code.</summary>
		/// <param name="statusCode">Status code.</param>
		public void AddStatusCode(int statusCode)
		{
			StatusCode = statusCode;
		}

		/// <summary>Adds the general message.</summary>
		/// <param name="message">Message.</param>
		public void AddMessage(string message)
		{
			Message = message;
		}
		/// <summary>Adds a result in the list of results.</summary>
		/// <param name="statusCode">Status code.</param>
		/// <param name="message">Message.</param>
		/// <param name="cardUID">Card unique identifier.</param>
		public void AddResult(int statusCode, string message, string cardUID)
		{
			Results.Add(new CardActionResult(statusCode, message, cardUID));
		}

		#endregion Public Methods
	}
}
