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
	public class DalType
	{
		private MagicAppContext _appContext;

		public DalType(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		public Models.Type? GetByValue(string value)
		{
			return _appContext.Types.FirstOrDefault(t => t.Value == value);
		}

		public void AddCardType(Card card, Models.Type type)
		{
			var cardType = new CardTypes
			{
				CardId = card.Id,
				Card = card,
				TypeId = type.Id,
				Type = type
			};
			_appContext.CardTypes.Add(cardType);
			_appContext.SaveChanges();
		}

		public void AddCardTypes(Card card, List<Models.Type> types)
		{
			foreach (var type in card.Types)
			{
				var currentType = GetByValue(type.Value);
				if (currentType != null)
					_appContext.CardTypes.Add(new CardTypes(card, currentType));
			}
			_appContext.SaveChanges();
		}
	}
}
