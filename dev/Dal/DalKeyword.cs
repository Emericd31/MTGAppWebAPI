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
	public class DalKeyword
	{
		private MagicAppContext _appContext;

		public DalKeyword(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		public Keyword? GetByValue(string keyword)
		{
			return _appContext.Keywords.FirstOrDefault(k => k.Value == keyword);
		}

		public void AddCardKeywords(Card card, List<Keyword> keywords)
		{
			foreach (var keyword in card.Keywords)
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

					//var cardKeyword = new CardKeywords
					//{
					//	CardId = card.Id,
					//	Card = card,
					//	KeywordId = newKeyword.Id,
					//	Keyword = newKeyword
					//};
					_appContext.CardKeywords.Add(new CardKeywords(card, newKeyword));
				}
			}

			_appContext.SaveChanges();
		}

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
	}
}
