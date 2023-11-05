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
	/// <summary>Data access layer for type/cardType objects.</summary>
	public class DalType
	{
		#region Private Properties

		/// <summary>Database context.</summary>
		private MagicAppContext _appContext;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public DalType(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		#endregion Constructor 

		#region Public Methods

		#region Getters

		/// <summary>Gets a type by value.</summary>
		/// <param name="value">Value.</param>
		/// <returns>The <see cref="Type"/> object (null if not found).</returns>
		public Models.Type? GetByValue(string value)
		{
			return _appContext.Types.FirstOrDefault(t => t.Value == value);
		}

		#endregion Getters

		#region Adding

		/// <summary>Adds a card type in database and saves the context.</summary>
		/// <param name="card">Card.</param>
		/// <param name="type">Type.</param>
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

		/// <summary>Adds a list of card types in database and saves the context.</summary>
		/// <param name="card">Card.</param>
		/// <param name="types">List of <see cref="Type"/>.</param>
		public void AddCardTypes(Card card, List<Models.Type> types)
		{
			foreach (var type in card.Types)
			{
				var currentType = GetByValue(type.Value ?? string.Empty);
				if (currentType != null)
					_appContext.CardTypes.Add(new CardTypes(card, currentType));
			}
			_appContext.SaveChanges();
		}

		#endregion Adding

		#endregion Public Methods
	}
}
