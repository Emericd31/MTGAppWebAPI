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
	/// <summary>Data access layer for color/cardColor objects.</summary>
	public class DalColor
	{
		#region Private Properties

		/// <summary>Database context.</summary>
		private MagicAppContext _appContext;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public DalColor(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		#endregion Constructor 

		#region Public Methods

		#region Getters

		/// <summary>Gets all colors.</summary>
		/// <returns>A list of <see cref="Color"/> objects.</returns>
		public IQueryable<Color> GetAll()
		{
			return _appContext.Colors;
		}

		/// <summary>Gets a color by value.</summary>
		/// <param name="value">Value.</param>
		/// <returns>A <see cref="Color"/> object (null if not found).</returns>
		public Color? GetByValue(string value)
		{
			return _appContext.Colors.FirstOrDefault(c => c.Value == value);
		}

		#endregion Getters

		#region Adding

		/// <summary>Adds a card color in database and saves the context.</summary>
		/// <param name="card">Card.</param>
		/// <param name="color">Color.</param>
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

		/// <summary>Adds a list of card colors in database and saves the context.</summary>
		/// <param name="card">Card.</param>
		/// <param name="colors">List of <see cref="Color"/>.</param>
		public void AddCardColors(Card card, List<Color> colors)
		{
			foreach (var color in colors)
			{
				var currentColor = GetByValue(color.Value);
				if (currentColor != null)
					_appContext.CardColors.Add(new CardColors(card, color));
			}
			_appContext.SaveChanges();
		}

		#endregion Adding

		#endregion Public Methods
	}
}
