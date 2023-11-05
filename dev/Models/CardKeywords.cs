/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

namespace MagicAppAPI.Models
{
	/// <summary>Card keyword object.</summary>
	public class CardKeywords
	{
		#region Public Properties

		/// <summary>Card's identifier.</summary>
		public int CardId { get; set; }

		/// <summary>Card object.</summary>
		public Card? Card { get; set; }

		/// <summary>Keyword's identifier.</summary>
		public int KeywordId { get; set; }

		/// <summary>Keyword object.</summary>
		public Keyword? Keyword { get; set; }

		#endregion Public Properties

		#region Constructor

		/// <summary>Default constructor.</summary>
		public CardKeywords() { }

		/// <summary>Constructor.</summary>
		/// <param name="card">Card.</param>
		/// <param name="keyword">Keyword.</param>
		public CardKeywords(Card card, Keyword keyword)
		{
			Card = card;
			CardId = card.Id;
			Keyword = keyword;
			KeywordId = keyword.Id;
		}

		#endregion Constructor
	}
}
