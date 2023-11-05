/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using MagicAppAPI.Context;
using MagicAppAPI.Models;

namespace MagicAppAPI.Dal
{
	/// <summary>Data access layer for keyword/cardKeyword objects.</summary>
	public class DalKeyword
	{
		#region Private Properties

		/// <summary>Database context.</summary>
		private MagicAppContext _appContext;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public DalKeyword(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		#endregion Constructor 

		#region Public Methods

		#region Getters

		/// <summary>Gets a keyword by value.</summary>
		/// <param name="value">Value.</param>
		/// <returns>A <see cref="Keyword"/> object (null if not found).</returns>
		public Keyword? GetByValue(string value)
		{
			return _appContext.Keywords.FirstOrDefault(k => k.Value == value);
		}

		#endregion Getters

		#region Adding

		/// <summary>Adds a card keyword in database and saves the context.</summary>
		/// <param name="card">Card.</param>
		/// <param name="keyword">Keyword.</param>
		public void AddCardKeyword(Card card, Keyword keyword)
		{
			var cardKeyword = new CardKeywords
			{
				CardId = card.Id,
				Card = card,
				KeywordId = keyword.Id,
				Keyword = keyword
			};
			_appContext.CardKeywords.Add(cardKeyword);
			_appContext.SaveChanges();
		}

		/// <summary>Adds a list of card keywords in database and saves the context.</summary>
		/// <param name="card">Card.</param>
		/// <param name="keywords">List of <see cref="Keyword"/>.</param>
		public void AddCardKeywords(Card card, List<Keyword> keywords)
		{
			foreach (var keyword in keywords)
			{
				var currentKeyword = GetByValue(keyword.Value);
				if (currentKeyword != null)
				{
					_appContext.CardKeywords.Add(new CardKeywords(card, keyword));
				}
				else
				{
					var newKeyword = new Keyword
					{
						Value = keyword.Value
					};
					_appContext.CardKeywords.Add(new CardKeywords(card, newKeyword));
				}
			}

			_appContext.SaveChanges();
		}

		#endregion Adding

		#endregion Public Methods
	}
}
