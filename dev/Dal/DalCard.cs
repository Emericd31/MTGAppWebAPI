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
	/// <summary>Data access layer for card objects.</summary>
	public class DalCard
	{
		#region Private Properties

		/// <summary>Database context.</summary>
		private MagicAppContext _appContext;

		#endregion Private Properties

		#region Constructor

		/// <summary>Constructor.</summary>
		/// <param name="appContext">Database context.</param>
		public DalCard(MagicAppContext appContext)
		{
			_appContext = appContext;
		}

		#endregion Constructor 

		#region Public Methods

		#region Getters

		/// <summary>Gets a dard by unique identifier.</summary>
		/// <param name="uid">Unique identifier.</param>
		/// <returns>A <see cref="Card"/> object (null if not found).</returns>
		public Card? GetByUID(string uid)
		{
			return _appContext.Cards.FirstOrDefault(c => c.UID == uid);
		}

		#endregion Getters

		#endregion Public Methods
	}
}
