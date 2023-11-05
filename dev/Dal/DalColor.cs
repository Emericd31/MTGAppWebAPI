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
	public class DalColor
	{
		private MagicAppContext _appContext;

		public DalColor(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		public IQueryable<Color> GetAllColors()
		{
			return _appContext.Colors;
		}

		public Color? GetColorByValue(string colorValue)
		{
			return _appContext.Colors.FirstOrDefault(c => c.Value == colorValue);
		}

		public void AddCardColor(Card card, Color color)
		{
			var cardColor = new CardColors
			{
				CardId = card.Id,
				Card = card,
				ColorId = color.Id,
				Color = color
			};
			_appContext.CardColors.Add(cardColor);
			_appContext.SaveChanges();
		}

		public void AddCardColors(Card card, List<Color> colors)
		{
			foreach (var color in colors)
			{
				var currentColor = GetColorByValue(color.Value);
				if (currentColor != null)
					_appContext.CardColors.Add(new CardColors(card, color));
			}
			_appContext.SaveChanges();
		}
	}
}
